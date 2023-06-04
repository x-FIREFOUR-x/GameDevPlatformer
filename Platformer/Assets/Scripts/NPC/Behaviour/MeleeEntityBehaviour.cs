using System;
using UnityEngine;

using Movement.Controller;
using Movement.Data;
using Movement.Enums;
using Core.Animation;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private AttackData _attackData;
        private Collider2D _entityCollider;
        
        [field: SerializeField] public LayerMask TargetsMask { get; private set; }
        [field: SerializeField] public Vector2 TargetSearchBox { get; private set; }

        private Attacker _attacker;
        
        public event Action AttackSequenceEnded;

        public Vector2 Size => _entityCollider.bounds.size;

        public override void Initialize()
        {
            base.Initialize();
            _entityCollider = GetComponent<BoxCollider2D>();
            Mover = new PositionMover(Rigidbody);
            _attacker = new Attacker(_attackData);
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

        public void Attack()
        {
            Animator.PlayAnimation(AnimationType.Attack, true);
            _attacker.Attack(true);
            EndAttack();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, TargetSearchBox);
        }

        public void SetDirection(Direction direction) => Mover.SetDirection(direction);

        private void EndAttack()
        {
            Invoke(nameof(EndAttackSequence), _attackData.TimeAttack);
        }

        private void EndAttackSequence()
        {
            Animator.PlayAnimation(AnimationType.Attack, false);
            AttackSequenceEnded?.Invoke();
        }
    }
}
