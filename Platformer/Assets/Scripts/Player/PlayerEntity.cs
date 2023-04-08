using UnityEngine;

using Core.Animation;
using Movement.Data;
using Movement.Controller;
using StatsSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerEntity : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        [SerializeField] private AnimationController _animator;

        [SerializeField] private MoveData _moveData;
        [SerializeField] private JumpData _jumperData;
        [SerializeField] private RollData _rollData;
        [SerializeField] private AttackData _attackData;

        private Mover _mover;
        private Jumper _jumper;
        private Roller _roller;
        private Attacker _attacker;
        private Blocker _blocker;

        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _mover = new Mover(_rigidbody, _moveData, statValueGiver);
            _jumper = new Jumper(_rigidbody, _jumperData, statValueGiver);
            _roller = new Roller(_rigidbody, GetComponent<BoxCollider2D>(), _rollData);
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
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _mover.MoveActive);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.JumpActive);
            _animator.PlayAnimation(AnimationType.Fall, _jumper.FallActive);
            _animator.PlayAnimation(AnimationType.Roll, _roller.RollActive);
            _animator.PlayAnimation(AnimationType.BlockIdle, _blocker.BlockActive);

            _animator.UpdateAnimationsAttack(_attacker.AttackActive);
        }

        public void Move(float direction) => _mover.Move(direction, CanMove());
        public void Jump() => _jumper.Jump(CanJump());
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
