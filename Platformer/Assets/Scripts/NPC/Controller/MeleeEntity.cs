using System.Collections;
using UnityEngine;
using Pathfinding;

using StatsSystem;
using StatsSystem.Enum;
using NPC.Behaviour;
using Core.Services.Updater;
using Fight;
using Movement.Enums;

namespace NPC.Controller
{
    [RequireComponent(typeof(Seeker))]
    public class MeleeEntity : Entity
    {
        private readonly MeleeEntityBehaviour _meleeEntityBehaviour;
        private readonly Seeker _pathSeeker;
        private readonly Vector2 _moveDelta;

        private Coroutine _searchCoroutine;
        private Collider2D _target;
        private Vector3 _previousTargetPosition;
        private Vector3 _destination;
        private float _stoppingDistance;
        private Path _currentPath;
        private int _indexCurrentPointInPath;


        public MeleeEntity(MeleeEntityBehaviour entityBehaviour, StatsController statsController) :
            base(entityBehaviour, statsController)
        {
            _pathSeeker = entityBehaviour.GetComponent<Seeker>();
            _meleeEntityBehaviour = entityBehaviour;

            var speedDelta = StatsController.GetStatValue(StatType.Speed) * Time.fixedDeltaTime;
            _moveDelta = new Vector2(speedDelta, 0);
            VisualiseHP(StatsController.GetStatValue(StatType.Health));

            _meleeEntityBehaviour.AttackSequenceEnded += OnAttackEnded;
            _meleeEntityBehaviour.Attacked += OnAttacked;

            _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchPathCoroutine());
            
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdateCalled;
        }
        
        public override void Dispose()
        {
            _meleeEntityBehaviour.AttackSequenceEnded -= OnAttackEnded;
            _meleeEntityBehaviour.Attacked -= OnAttacked;
            
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdateCalled;
            ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);
            
            base.Dispose();
        }

        private IEnumerator SearchPathCoroutine()
        {
            while(!IsAttacking)
            {
                if(!TryGetTarget(out _target))
                { 
                    ResetMovement();
                }
                else if (_target.transform.position != _previousTargetPosition)
                {
                    Vector2 position = _target.transform.position;
                    _previousTargetPosition = position;
                    _stoppingDistance = (_target.bounds.size.x + _meleeEntityBehaviour.Size.x) / 2;
                    var delta = position.x < _meleeEntityBehaviour.transform.position.x ? 1 : -1;
                    _destination = position + new Vector2(_stoppingDistance * delta, 0);
                    _pathSeeker.StartPath(_meleeEntityBehaviour.transform.position, _destination, OnPathCalculated);
                }       

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnPathCalculated(Path path)
        {
            if (path.error)
                return;

            _currentPath = path;
            _indexCurrentPointInPath = 0;
        }

        private bool TryGetTarget(out Collider2D target)
        {
            target = null;
            if (_meleeEntityBehaviour == null || _meleeEntityBehaviour.transform == null)
                return false;
            
            target = Physics2D.OverlapBox(_meleeEntityBehaviour.transform.position, _meleeEntityBehaviour.TargetSearchBox, 0,
                _meleeEntityBehaviour.TargetsMask);

            return target != null;
        }

        private void OnFixedUpdateCalled()
        {
            if (IsAttacking || _target == null || _currentPath == null || TryAttack() || _indexCurrentPointInPath >= _currentPath.vectorPath.Count)
            {
                ResetMovement();
                return;
            }

            var currentPosition = _meleeEntityBehaviour.transform.position;
            var positionPointInPath = _currentPath.vectorPath[_indexCurrentPointInPath];
            var finalPositionPointInPath = _currentPath.vectorPath[_currentPath.vectorPath.Count - 1];
            var directionPointInPath = positionPointInPath - currentPosition;

            if (Mathf.Abs(positionPointInPath.x  - currentPosition.x) < 0.05f)
            {
                _indexCurrentPointInPath++;
                return;
            }

            if (directionPointInPath.x == 0)
                return;

            directionPointInPath.x = directionPointInPath.x > 0 ? 1 : -1;
            float newHorizontalPosition = currentPosition.x + _moveDelta.x * directionPointInPath.x;

            if (directionPointInPath.x > 0 && positionPointInPath.x < newHorizontalPosition ||
               directionPointInPath.x < 0 && positionPointInPath.x > newHorizontalPosition)
            {
                newHorizontalPosition = positionPointInPath.x;
            }

            if (newHorizontalPosition != _meleeEntityBehaviour.transform.position.x)
                _meleeEntityBehaviour.Move(newHorizontalPosition, finalPositionPointInPath.x);
        }

        private bool TryAttack()
        {
            var distance = _destination - _meleeEntityBehaviour.transform.position;
            if (Mathf.Abs(distance.x) > _meleeEntityBehaviour.AttackRadius)
                return false;

            _meleeEntityBehaviour.SetDirection(_meleeEntityBehaviour.transform.position.x > _target.transform.position.x ? Direction.Left : Direction.Right);
            ResetMovement();

            IsAttacking = true;
            _meleeEntityBehaviour.Attack();

            if (_searchCoroutine != null)
                ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);

            return true;
        }

        private void ResetMovement()
        {
            _target = null;
            _currentPath = null;
            _previousTargetPosition = Vector2.negativeInfinity;
            var position = _meleeEntityBehaviour.transform.position;
            _meleeEntityBehaviour.Move(position.x, position.x);
        }

        private void OnAttacked(IDamageable target)
        {
            target.TakeDamage(StatsController.GetStatValue(StatType.Damage));
        }

        private void OnAttackEnded()
        {
            IsAttacking = false;
            ProjectUpdater.Instance.Invoke(() =>
            {
                _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchPathCoroutine());
            }, StatsController.GetStatValue(StatType.AfterAttackDelay));
            
        }

        protected sealed override void VisualiseHP(float currentHp)
        {
            if (_meleeEntityBehaviour.HPBar.maxValue < currentHp)
                _meleeEntityBehaviour.HPBar.maxValue = currentHp;

            _meleeEntityBehaviour.HPBar.value = currentHp;
        }
    }
}
