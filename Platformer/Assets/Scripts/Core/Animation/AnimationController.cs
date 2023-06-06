using System;
using UnityEngine;

namespace Core.Animation
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private AnimationType _currentAnimationType;

        private readonly AnimationType _firstAttackAnimation = AnimationType.Attack;
        private readonly AnimationType _lastAttackAnimation = AnimationType.Attack3;
        private Action _animationStartAction;
        private Action _animationEndAction;

        private AnimationType _lastShowAttackAnim = AnimationType.Attack;

        public void  UpdateAnimationsAttack(bool attackActive)
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
        
        public bool SetAnimationState(AnimationType animationType, bool active, Action startAnimationAction = null, Action endAnimationAction = null)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                    return false;
                
                OnAnimationEnded();
                return false;
            }

            if (_currentAnimationType >= animationType)
                return false;

            _animationStartAction = startAnimationAction;
            _animationEndAction = endAnimationAction;

            if ((int)animationType <= (int)_firstAttackAnimation || (int)animationType > (int)_lastAttackAnimation)
            {
                SetAnimation(animationType); 
            }
            else
            {
                UpdateAnimationsAttack(true);
            }
            
            return true;
        }

        protected void OnActionRequested() => _animationStartAction?.Invoke();
        
        private void OnAnimationEnded()
        {
            _animationEndAction?.Invoke();
            _animationStartAction = null;
            _animationEndAction = null;
            SetAnimation(AnimationType.Idle);
        }

        private void SetAnimation(AnimationType animationType)
        {
            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
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
