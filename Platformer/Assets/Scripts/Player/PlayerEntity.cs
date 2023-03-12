using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _faceRight;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;

        private Rigidbody2D _rigidbody;

        public Vector2 Velocity { get { return _rigidbody.velocity; } }
        public bool BlockActive { get; private set; }


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }


        public void Move(float direction)
        {
            Vector2 velocity = _rigidbody.velocity;
            if (IsCanMove())
            {
                velocity.x = direction * _moveSpeed;
                SetDirectionSprite(direction);
            }
            else
            {
                velocity.x = 0;
            }
            
            _rigidbody.velocity = velocity;
        }

        public void Jump()
        {
            if (!IsCanJump())
                return;

            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _rigidbody.gravityScale = _gravityScale;
        }

        public void Block(bool activeBlock)
        {
            BlockActive = activeBlock && IsCanBlock();
        }


        private bool IsCanMove()
        {
            return !BlockActive;
        }

        private bool IsCanJump()
        {
            return _rigidbody.velocity.y == 0 && !BlockActive;
        }

        private bool IsCanBlock()
        {
            return _rigidbody.velocity.y == 0;
        }


        private void SetDirectionSprite(float direction)
        {
            if ((_faceRight && direction < 0) || (!_faceRight && direction > 0))
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
    }

}
