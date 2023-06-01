using UnityEngine;

using Core.Animation;
using Movement.Data;
using Movement.Controller;
using NPC.Behaviour;
using StatsSystem;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerEntityBehaviour : BaseEntityBehaviour
    {

        [SerializeField] private JumpData _jumperData;
        [SerializeField] private RollData _rollData;
        [SerializeField] private AttackData _attackData;
        
        private Jumper _jumper;
        private Roller _roller;
        private Attacker _attacker;
        private Blocker _blocker;

        public override void Initialize()
        {
            base.Initialize();
            _jumper = new Jumper(Rigidbody, _jumperData);
            _roller = new Roller(Rigidbody, GetComponent<BoxCollider2D>(), _rollData);
            _attacker = new Attacker(_attackData);
            _blocker = new Blocker();
        }

        private void Update()
        {
            UpdateAnimations();

            _roller.UpdateRoll();
            _attacker.UpdateAtack();
        }

        public void UpdateAnimations()
        {
            Animator.PlayAnimation(AnimationType.Idle, true);
            Animator.PlayAnimation(AnimationType.Run, BaseMover.MoveActive);
            Animator.PlayAnimation(AnimationType.Jump, _jumper.JumpActive);
            Animator.PlayAnimation(AnimationType.Fall, _jumper.FallActive);
            Animator.PlayAnimation(AnimationType.Roll, _roller.RollActive);
            Animator.PlayAnimation(AnimationType.BlockIdle, _blocker.BlockActive);

            Animator.UpdateAnimationsAttack(_attacker.AttackActive);
        }

        public override void Move(float direction)
        {
            if (CanMove())
            {
                base.Move(direction);
            }
        }
        public void Jump(float jumpForce) => _jumper.Jump(CanJump(),jumpForce);
        public void Roll() => _roller.Roll(CanRoll());
        public void Block(bool activeBlock) => _blocker.Block(activeBlock && CanBlock());
        public void Attack() => _attacker.Attack(CanAttack());
        

        private bool CanMove()
        {
            return !_blocker.BlockActive && !_roller.RollActive;
        }

        private bool CanJump()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_blocker.BlockActive && !_roller.RollActive && !_attacker.AttackActive;
        }

        private bool CanRoll()
        {
            return !_blocker.BlockActive && !_roller.RollActive && !_attacker.AttackActive;
        }

        private bool CanBlock()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_roller.RollActive && !_attacker.AttackActive;
        }

        private bool CanAttack()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_roller.RollActive && !_attacker.AttackActive;
        }
    }
}
