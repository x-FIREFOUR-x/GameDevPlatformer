using UnityEngine;

using Movement.Data;

namespace Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly JumpData _jumpData;
        
        public bool JumpActive
        {
            get { return (_rigidbody.velocity.y > 0 && !IsGrounded()); }
        }
        public bool FallActive
        {
            get { return (_rigidbody.velocity.y < 0 && !IsGrounded()); }
        }

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
            
        }

        public void Jump(bool isCanJump, float jumpForce)
        {
            if (!isCanJump)
                return;

            _rigidbody.AddForce(Vector2.up * jumpForce );
            _rigidbody.gravityScale = _jumpData.GravityScale;
        }
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(_jumpData.GroundCheck.position, 0.2f, _jumpData.GroundLayer);
        }
    }
}
