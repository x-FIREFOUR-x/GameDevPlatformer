using UnityEngine;

using Movement.Data;

namespace Movement.Controller
{
    public class Roller
    {
        private readonly Transform _transform;

        private BoxCollider2D _collider;
        private Vector2 _offsetFullCollider;
        private Vector2 _sizeFullCollider;

        private RollData _rollData;

        private float _remainingRoleDistance;


        public bool RollActive { get; private set; }


        public Roller(Rigidbody2D _rigidbody, BoxCollider2D collider, RollData rollData)
        {
            _transform = _rigidbody.transform;

            _collider = collider;
            _offsetFullCollider = _collider.offset;
            _sizeFullCollider = _collider.size;

            _rollData = rollData;
        }


        public void Roll(bool isCanRoll)
        {
            if (!isCanRoll)
                return;

            _remainingRoleDistance = _rollData.RollDistance;

            SwitchStateRoll();
        }

        public void UpdateRoll()
        {
            if (RollActive)
            {
                float distance = _rollData.RollSpeed * Time.deltaTime;
                _remainingRoleDistance -= distance;

                float directionFace = _transform.right.x;

                Vector3 position = _transform.position;
                position.x = position.x + distance * directionFace;
                _transform.position = position;

                if (_remainingRoleDistance <= 0)
                    SwitchStateRoll();
            }
        }

        private void SwitchStateRoll()
        {
            RollActive = !RollActive;

            if (RollActive)
                ChangeCollider(_rollData.SizeRollCollider, _rollData.OffsetRollCollider);
            else
                ChangeCollider(_sizeFullCollider, _offsetFullCollider);
        }

        private void ChangeCollider(Vector2 size, Vector2 offset)
        {
            _collider.size = size;
            _collider.offset = offset;
        }
    }
}
