using UnityEngine;

using Core.Animation;
using Movement.Controller.Movers;
using Movement.Data;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimationController Animator;

        protected Rigidbody2D Rigidbody;
        protected BaseMover Mover;
        
        public virtual void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Mover = new VelocityMover(Rigidbody);
        }

        protected virtual void UpdateAnimations()
        {
            Animator.PlayAnimation(AnimationType.Idle, true);
            Animator.PlayAnimation(AnimationType.Run, Mover.MoveActive);
        }
        public abstract void Move(float direction);
    }
}
