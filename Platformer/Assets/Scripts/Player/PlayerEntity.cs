using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerEntity : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private BoxCollider2D _collider;
        private Vector2 _offsetFullCollider;
        private Vector2 _sizeFullCollider;

        [Header("Move")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _faceRight;


        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;


        [Header("Roll")]
        [SerializeField] private float _rollSpeed;
        [SerializeField] private float _rollDistance;
        private float _remainingRoleDistance;
        private int _diretionRoll;

        [SerializeField] private Vector2 _offsetRollCollider;
        [SerializeField] private Vector2 _sizeRollCollider;
        

        public Vector2 Velocity { get { return _rigidbody.velocity; } }
        public bool RollActive { get; private set; }
        public bool BlockActive { get; private set; }

        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _collider = GetComponent<BoxCollider2D>();
            _offsetFullCollider = _collider.offset;
            _sizeFullCollider = _collider.size;
        }

        private void Update()
        {
            UpdateRoll();
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

        public void Roll()
        {
            if (!IsCanRoll())
                return;
            
            _remainingRoleDistance = _rollDistance;
            _diretionRoll = _faceRight ? 1 : -1;

            SwitchStateRoll();
        }

        public void Block(bool activeBlock)
        {
            BlockActive = activeBlock && IsCanBlock();
        }


        private bool IsCanMove()
        {
            return !BlockActive && !RollActive;
        }

        private bool IsCanJump()
        {
            return _rigidbody.velocity.y == 0 && !BlockActive && !RollActive;
        }

        private bool IsCanRoll()
        {
            return _rigidbody.velocity.y == 0 && !BlockActive && !RollActive;
        }

        private bool IsCanBlock()
        {
            return _rigidbody.velocity.y == 0 && !RollActive;
        }


        private void SetDirectionSprite(float direction)
        {
            if ((_faceRight && direction < 0) || (!_faceRight && direction > 0))
                FlipTransform();
        }

        private void FlipTransform()
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }


        private void UpdateRoll()
        {
            if(RollActive)
            {
                float distance = _rollSpeed * Time.deltaTime;
                _remainingRoleDistance -= distance;

                Vector3 position = transform.position;
                position.x = position.x + distance * _diretionRoll;
                transform.position = position;

                if(_remainingRoleDistance <= 0)
                    SwitchStateRoll();
                
            }
        }

        private void SwitchStateRoll()
        {
            RollActive = !RollActive;

            if (RollActive)
                ChangeCollider(_sizeRollCollider, _offsetRollCollider);
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
