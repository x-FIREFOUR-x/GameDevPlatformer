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


        [Header("Attack")]
        [SerializeField] private float _timeAttack;
        private float _remainingTimeAttack;

        private Vector2 Velocity => _rigidbody.velocity;
        
        public bool MoveActive { get { return Velocity.x != 0; } }
        public bool JumpActive { get { return Velocity.y > 0; }}
        public bool FallActive { get { return Velocity.y < 0; } }
        public bool RollActive { get; private set; }
        public bool BlockActive { get; private set; }
        public bool AttackActive { get; private set; }


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
            UpdateAtack();
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

        public void Attack()
        {
            if (!IsCanAttack())
                return;

            AttackActive = true;
            _remainingTimeAttack = _timeAttack;
        }


        private bool IsCanMove()
        {
            return !BlockActive && !RollActive;
        }

        private bool IsCanJump()
        {
            return !JumpActive && !FallActive && !BlockActive && !RollActive && !AttackActive;
        }

        private bool IsCanRoll()
        {
            return !BlockActive && !RollActive && !AttackActive;
        }

        private bool IsCanBlock()
        {
            return !JumpActive && !FallActive && !RollActive && !AttackActive;
        }

        private bool IsCanAttack()
        {
            return !JumpActive && !FallActive && !RollActive && !AttackActive;
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


        private void UpdateAtack()
        {
            if (AttackActive)
            {
                _remainingTimeAttack -= Time.deltaTime;

                if(_remainingTimeAttack <= 0)
                    AttackActive = false;
            }
        }
    }

}
