using System;
using UnityEngine;

using Movement.Controller;
using Movement.Enums;

namespace NPC.Behaviour
{
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        [SerializeField] private float _attackSpeed;
        [SerializeField] private Collider2D _entityCollider;
        
        [field: SerializeField] public LayerMask TargetsMask { get; private set; }
        [field: SerializeField] public Vector2 TargetSearchBox { get; private set; }
        
        public event Action AttackSequenceEnded;
        
        public override void Initialize()
        {
            base.Initialize();
            Mover = new PositionMover(Rigidbody);
        }
        
        public override void Move(float direction)
        {
            Mover.Move(direction, true);
        }
        
        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, TargetSearchBox);
        }


        private void Update()
        {
            UpdateAnimations();
        }

        public void StartAttack() => Animator.UpdateAnimationsAttack(true);
        
        public void SetDirection(Direction direction) => Mover.SetDirection(direction);

        private void EndAttack()
        {
            Animator.UpdateAnimationsAttack(false);
            Invoke(nameof(EndAttackSequence), 1 / _attackSpeed);
        }

        private void EndAttackSequence()
        {
            AttackSequenceEnded?.Invoke();
        }
    }
}