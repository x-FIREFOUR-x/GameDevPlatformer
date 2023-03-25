using UnityEngine;

using Animation;
using Movement.Data;
using Movement.Controller;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerEntity : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;


        [SerializeField] private PlayerAnimatorController _animator;


        [SerializeField] private MovementData _movementData;
        private Mover _mover;

        [SerializeField] private JumpData _jumperData;
        private Jumper _jumper;

        [SerializeField] private RollData _rollData;
        private Roller _roller;

        [SerializeField] private AttackData _attackData;
        private Attacker _attacker;

        private Blocker _blocker;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _mover = new Mover(_rigidbody, _movementData);
            _jumper = new Jumper(_rigidbody, _jumperData);
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


        public void Move(float direction) => _mover.Move(direction, IsCanMove());
        public void Jump() => _jumper.Jump(IsCanJump());
        public void Roll() => _roller.Roll(IsCanRoll());
        public void Block(bool activeBlock) => _blocker.Block(activeBlock && IsCanBlock());
        public void Attack() => _attacker.Attack(IsCanAttack());
        

        private bool IsCanMove()
        {
            return !_blocker.BlockActive && !_roller.RollActive;
        }

        private bool IsCanJump()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_blocker.BlockActive && !_roller.RollActive && !_attacker.AttackActive;
        }

        private bool IsCanRoll()
        {
            return !_blocker.BlockActive && !_roller.RollActive && !_attacker.AttackActive;
        }

        private bool IsCanBlock()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_roller.RollActive && !_attacker.AttackActive;
        }

        private bool IsCanAttack()
        {
            return !_jumper.JumpActive && !_jumper.FallActive && !_roller.RollActive && !_attacker.AttackActive;
        }
    }

}
