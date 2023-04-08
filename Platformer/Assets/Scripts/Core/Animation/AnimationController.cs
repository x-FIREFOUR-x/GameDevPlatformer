using UnityEngine;

namespace Core.Animation
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private AnimationType _currentAnimationType;

        private readonly AnimationType _firstAttackAnimation = AnimationType.Attack;
        private readonly AnimationType _lastAttackAnimation = AnimationType.Attack3;

        private AnimationType _lastShowAttackAnim = AnimationType.Attack;

        public void UpdateAnimationsAttack(bool attackActive)
        {
            if(!attackActive)
            {
                for (int i = (int)_firstAttackAnimation; i <= (int)_lastAttackAnimation; i++)
                {
                    PlayAnimation((AnimationType)i, attackActive);
                }
            }
            else
            {
                if(!(_currentAnimationType >= _firstAttackAnimation && _currentAnimationType <= _lastAttackAnimation))
                {
                    AnimationType randomAttackAnim = RandomAttackAnimation();
                    PlayAnimation(randomAttackAnim, attackActive);

                    _lastShowAttackAnim = randomAttackAnim;
                }
            }
        }
        
        public void PlayAnimation(AnimationType animationType, bool active)
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
                randomAttackAnim = UnityEngine.Random.Range((int)_firstAttackAnimation, (int)_lastAttackAnimation + 1);

                if (randomAttackAnim != (int)_lastShowAttackAnim)
                    randomCorrect = true;
            }

            return (AnimationType)randomAttackAnim;
        }
    }
}
