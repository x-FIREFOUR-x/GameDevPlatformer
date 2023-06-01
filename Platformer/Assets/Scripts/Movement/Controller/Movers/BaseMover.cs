using UnityEngine;

using Movement.Enums;

namespace Movement.Controller.Movers
{
    public abstract class BaseMover
    {
        public Direction Direction { get; private set; }
        public abstract bool MoveActive { get; }
        
        protected readonly Rigidbody2D Rigidbody;
        
        public BaseMover(Rigidbody2D rigidbody)
        {
            Direction = Direction.Right;
            Rigidbody = rigidbody;
        }

        public abstract void Move(float horizontalMovement, bool isCanMove);

        public void SetDirection(Direction newDirection)
        {
            if (Direction == newDirection) 
                return;
            
            Rigidbody.transform.Rotate(0, 180, 0);
            Direction = newDirection;
        }
    }
}
