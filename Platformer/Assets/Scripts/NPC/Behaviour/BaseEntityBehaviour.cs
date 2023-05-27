using UnityEngine;

using Core.Animation;
using Movement.Controller;
using Movement.Data;
using StatsSystem;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseEntityBehaviour : MonoBehaviour
    {
        [SerializeField] protected AnimationController Animator;
        [SerializeField] private MoveData _moveData;

        protected Rigidbody2D Rigidbody;
        protected Mover Mover;
        
        public virtual void Initialize(IStatValueGiver statValueGiver)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Mover = new Mover(Rigidbody, _moveData, statValueGiver);
        }
        
        public virtual void Move(float direction) => Mover.Move(direction, true);
        
    }
}
