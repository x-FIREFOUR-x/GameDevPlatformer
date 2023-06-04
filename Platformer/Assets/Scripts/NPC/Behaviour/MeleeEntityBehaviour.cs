using System;
using UnityEngine;
using UnityEngine.UI;

using Movement.Controller;
using Movement.Data;
using Movement.Enums;
using Core.Animation;
using Fight;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        private Collider2D _entityCollider;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRadius;
        
        [field: SerializeField] public LayerMask TargetsMask { get; private set; }
        [field: SerializeField] public Vector2 TargetSearchBox { get; private set; }
        [field: SerializeField] public Slider HPBar { get; private set; }
        
        public event Action<IDamageable> Attacked;
        public event Action AttackSequenceEnded;

        public Vector2 Size => _entityCollider.bounds.size;

        public override void Initialize()
        {
            base.Initialize();
            _entityCollider = GetComponent<BoxCollider2D>();
            Mover = new PositionMover(Rigidbody);
        }

        private void Update()
        {
            UpdateAnimations();
        }
        
        public override void Move(float direction)
        {
            Mover.Move(direction, true);
        }

        public void Move(float direction, float finalDirection)
        {
            ((PositionMover)Mover).Move(direction, finalDirection);
        }

        public void StartAttack() => Animator.SetAnimationState(AnimationType.Attack, true, Attack, EndAttack);
        
        private void Attack()
        {
            var targetCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, TargetsMask);
            if (targetCollider != null && targetCollider.TryGetComponent(out IDamageable damageable))
                Attacked?.Invoke(damageable);
        }
        
        private void EndAttack() => AttackSequenceEnded?.Invoke();

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, TargetSearchBox);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        }

        public void SetDirection(Direction direction) => Mover.SetDirection(direction);
    }
}
