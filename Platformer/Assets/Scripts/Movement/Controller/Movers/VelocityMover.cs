using Movement.Enums;
using UnityEngine;

namespace Movement.Controller.Movers
{
    public class VelocityMover : BaseMover
    {
        private Vector2 _movement;
        
        public override bool MoveActive => Rigidbody.velocity.x != 0;
        
        public VelocityMover(Rigidbody2D rigidbody) : base(rigidbody){}

        public override void Move(float horizontalMovement, bool isCanMove)
        {
            Vector2 velocity = Rigidbody.velocity;
            if (isCanMove)
            {
                velocity.x = horizontalMovement;
                SetDirection(horizontalMovement > 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                velocity.x = 0;
            }

            Rigidbody.velocity = velocity;
        }
    }
}
