using UnityEngine;

using Movement.Data;
using StatsSystem;
using StatsSystem.Enum;

namespace Movement.Controller
{
    public class Mover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly MoveData _movementData;

        private readonly IStatValueGiver _statValueGiver;

        public bool MoveActive { get { return _rigidbody.velocity.x != 0; } }

        public Mover(Rigidbody2D rigidbody, MoveData movementData, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _transform = _rigidbody.transform;
            _movementData = movementData;

            _statValueGiver = statValueGiver;
        }

        public void Move(float direction, bool isCanMove)
        {
            Vector2 velocity = _rigidbody.velocity;
            if (isCanMove)
            {
                velocity.x = direction * _statValueGiver.GetStatValue(StatType.Speed);
                SetDirectionSprite(direction);
            }
            else
            {
                velocity.x = 0;
            }

            _rigidbody.velocity = velocity;
        }

        private void SetDirectionSprite(float direction)
        {
            if ((_movementData.FaceRight && direction < 0) || (!_movementData.FaceRight && direction > 0))
                FlipTransform();
        }

        private void FlipTransform()
        {
            _transform.Rotate(0, 180, 0);
            _movementData.FaceRight = !_movementData.FaceRight;
        }
    }
}
