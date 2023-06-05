using System;
using UnityEngine;

using Core.Animation;
using Fight;
using Movement.Controller.Movers;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEntityBehaviour : MonoBehaviour, IDamageable
    {
        [SerializeField] protected AnimationController Animator;

        protected Rigidbody2D Rigidbody;
        protected BaseMover Mover;
        
        public event Action<float> DamageTaken;
        
        public virtual void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Mover = new VelocityMover(Rigidbody);
        }
        
        public abstract void Move(float direction);
        
        public void TakeDamage(float damage)
        {
            DamageTaken?.Invoke(damage);
        }
        
        public void PlayDeath()
        {
            Animator.SetAnimationState(AnimationType.Death, true, endAnimationAction: () =>
            {
                Destroy(gameObject);
            });
        }

        protected virtual void UpdateAnimations()
        {
            Animator.PlayAnimation(AnimationType.Idle, true);
            Animator.PlayAnimation(AnimationType.Run, Mover.MoveActive);
        }
    }
}
