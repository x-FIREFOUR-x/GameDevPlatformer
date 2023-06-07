using UnityEngine;

using Movement.Controller.Movers;
using Movement.Enums;

namespace Movement.Controller
{
    public class PositionMover : BaseMover
    {
        private Vector2 _destination;
        private Vector2 _finalDestination;

        public override bool MoveActive => _finalDestination.x != Rigidbody.position.x;


        public PositionMover(Rigidbody2D rigidbody) : base(rigidbody) 
        {
            _destination = Rigidbody.position;
        }

        public override void Move(float horizontalMovement, bool isCanMove)
        {
            if (isCanMove)
            {
                _destination.x = horizontalMovement;
                var newPosition = new Vector2(horizontalMovement, Rigidbody.position.y);
                
                _finalDestination = newPosition;

                Rigidbody.MovePosition(newPosition);
                if (_destination.x != Rigidbody.position.x)
                    SetDirection(_destination.x > Rigidbody.position.x ? Direction.Right : Direction.Left);
            }
        }

        public void Move(float horizontalMovement, float finalDestination)
        {
            if (Rigidbody == null)
                return;
            
            var newFinalPosition = new Vector2(finalDestination, Rigidbody.position.y);
            _finalDestination = newFinalPosition;

            _destination.x = horizontalMovement;
            var newPosition = new Vector2(horizontalMovement, Rigidbody.position.y);

            Rigidbody.MovePosition(newPosition);
            if (_destination.x != Rigidbody.position.x)
                SetDirection(_destination.x > Rigidbody.position.x ? Direction.Right : Direction.Left);
        }
    }
}
