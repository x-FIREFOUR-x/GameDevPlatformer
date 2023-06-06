using System;
using UnityEngine;

using Core.Animation;
using Fight;
using Movement.Data;
using Movement.Controller;
using NPC.Behaviour;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerEntityBehaviour : BaseEntityBehaviour
    {

        [SerializeField] private JumpData _jumperData;
        [SerializeField] private RollData _rollData;
        
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRadius;
        
        [field: SerializeField] public LayerMask TargetsMask { get; private set; }

        [field: SerializeField] public PlayerStatsUIView statsUIView { get; private set; }
        
        private Jumper _jumper;
        private Roller _roller;
        private Blocker _blocker;
        private bool _isAttacking;

        public event Action<IDamageable> Attacked;
        public event Action AttackEnded;

        public override void Initialize()
        {
            base.Initialize();
            _jumper = new Jumper(Rigidbody, _jumperData);
            _roller = new Roller(Rigidbody, GetComponent<BoxCollider2D>(), _rollData);
            _blocker = new Blocker();
        }

        private void Update()
        {
            UpdateAnimations();

            _roller.UpdateRoll();
        }

        override protected void UpdateAnimations()
        {
            base.UpdateAnimations();
            Animator.PlayAnimation(AnimationType.Jump, _jumper.JumpActive);
            Animator.PlayAnimation(AnimationType.Fall, _jumper.FallActive);
            Animator.PlayAnimation(AnimationType.Roll, _roller.RollActive);
            Animator.PlayAnimation(AnimationType.BlockIdle, _blocker.BlockActive);
        }

        public override void Move(float direction) => Mover.Move(direction, CanMove());
        public void Jump(float jumpForce) => _jumper.Jump(CanJump(), jumpForce);
        public void Roll() => _roller.Roll(CanRoll());
        public void Block(bool activeBlock) => _blocker.Block(activeBlock && CanBlock());

        public bool TryStartAttack()
        {
            if (CanAttack())
            {
                Animator.SetAnimationState(AnimationType.Attack, true, OnAttack, OnAttackEnded, true);
                return true;
            }

            return false;
        }

        private void OnAttack()
        {
            _isAttacking = true;
            var targetCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, TargetsMask);
            if (targetCollider != null && targetCollider.TryGetComponent(out IDamageable damageable))
                Attacked?.Invoke(damageable);
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            AttackEnded?.Invoke();
        }

        private bool CanMove()
        {
            return !_blocker.BlockActive && !_roller.RollActive;
        }

        private bool CanJump()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_blocker.BlockActive && !_roller.RollActive && !_isAttacking;
        }

        private bool CanRoll()
        {
            return !_blocker.BlockActive && !_roller.RollActive && !_isAttacking;
        }

        private bool CanBlock()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_roller.RollActive && !_isAttacking;
        }

        private bool CanAttack()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_roller.RollActive && !_isAttacking;
        }
    }
}
