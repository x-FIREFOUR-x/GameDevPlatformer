using Movement.Data;
using Movement.Enums;
using UnityEngine;

namespace Movement.Controller.Movers
{
    public abstract class BaseMover
    {
        protected readonly Rigidbody2D Rigidbody;
        public Direction Direction { get; private set; }

        public abstract bool MoveActive { get; }

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
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}
