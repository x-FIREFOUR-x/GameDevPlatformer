using Movement.Controller.Movers;
using Movement.Enums;
using UnityEngine;

namespace Movement.Controller
{
    public class PositionMover : BaseMover
    {
        private Vector2 _destination;
        public override bool MoveActive => _destination != Rigidbody.position;

        public PositionMover(Rigidbody2D rigidbody) : base(rigidbody) { }

        public override void Move(float horizontalMovement, bool isCanMove)
        {
            if (isCanMove)
            {
                _destination.x = horizontalMovement;
                var newPosition = new Vector2(horizontalMovement, Rigidbody.position.y);
                Rigidbody.MovePosition(newPosition);
                if(_destination.x != Rigidbody.position.x)
                    SetDirection(_destination.x > Rigidbody.position.x ? Direction.Right : Direction.Left);
            }
        }
    }
}
