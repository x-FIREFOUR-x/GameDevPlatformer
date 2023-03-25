using UnityEngine;

using Movement.Data;

namespace Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly JumpData _jumpData;


        public bool JumpActive { get { return _rigidbody.velocity.y > 0; } }
        public bool FallActive { get { return _rigidbody.velocity.y < 0; } }


        public Jumper(Rigidbody2D rigidbody, JumpData jumpData)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
        }

        public void Jump(bool isCanJump)
        {
            if (!isCanJump)
                return;

            _rigidbody.AddForce(Vector2.up * _jumpData.JumpForce);
            _rigidbody.gravityScale = _jumpData.GravityScale;
        }
    }
}
