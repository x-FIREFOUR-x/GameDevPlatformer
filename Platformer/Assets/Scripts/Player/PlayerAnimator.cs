using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _player;
        [SerializeField] private Animator _animator;

        private AnimationType _currentAnimationType;


        private AnimationType _firstAttackAnimation = AnimationType.Attack;
        private AnimationType _lastAttackAnimation = AnimationType.Attack3;

        private AnimationType _lastShowAttackAnim = AnimationType.Attack;

        private void FixedUpdate()
        {
            UpdateAnimations();
        }


        private void UpdateAnimations()
        {
            PlayAnimation(AnimationType.Idle, true);
            PlayAnimation(AnimationType.Run, _player.MoveActive);
            PlayAnimation(AnimationType.Jump, _player.JumpActive);
            PlayAnimation(AnimationType.Fall, _player.FallActive);
            PlayAnimation(AnimationType.Roll, _player.RollActive);
            PlayAnimation(AnimationType.BlockIdle, _player.BlockActive);

            UpdateAnimationsAttack();
        }

        private void UpdateAnimationsAttack()
        {
            if(!_player.AttackActive)
            {
                for (int i = (int)_firstAttackAnimation; i <= (int)_lastAttackAnimation; i++)
                {
                    PlayAnimation((AnimationType)i, _player.AttackActive);
                }
            }
            else
            {
                if(!(_currentAnimationType >= _firstAttackAnimation && _currentAnimationType <= _lastAttackAnimation))
                {
                    AnimationType randomAttackAnim = RandomAttackAnimation();
                    PlayAnimation(randomAttackAnim, _player.AttackActive);

                    _lastShowAttackAnim = randomAttackAnim;
                }
            }
        }
        
        private void PlayAnimation(AnimationType animationType, bool active)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return;

                _currentAnimationType = AnimationType.Idle;
                PlayAnimation(_currentAnimationType);
                return;
            }

            if (_currentAnimationType >= animationType)
                return;

            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
        }

        private void PlayAnimation(AnimationType animationType)
        {
            _animator.SetInteger(nameof(AnimationType), (int)animationType);
        }

        private AnimationType RandomAttackAnimation()
        {
            int randomAttackAnim = (int)_firstAttackAnimation;

            bool randomCorrect = false;
            while (!randomCorrect)
            {
                randomAttackAnim = Random.Range((int)_firstAttackAnimation, (int)_lastAttackAnimation + 1);

                if (randomAttackAnim != (int)_lastShowAttackAnim)
                    randomCorrect = true;
            }

            return (AnimationType)randomAttackAnim;
        }
    }

}
