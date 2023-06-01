using UnityEngine;

using Core.Animation;
using Movement.Controller.Movers;
using Movement.Data;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimationController Animator;
        [SerializeField] private MoveData _moveData;

        protected Rigidbody2D Rigidbody;
        protected BaseMover BaseMover;
        
        public virtual void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            BaseMover = new VelocityMover(Rigidbody);
        }
        
        public virtual void Move(float direction) => BaseMover.Move(direction, true);
        
    }
}
