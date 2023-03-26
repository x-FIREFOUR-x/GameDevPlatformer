using UnityEngine;

using Movement.Data;
using StatsSystem;
using StatsSystem.Enum;

namespace Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly JumpData _jumpData;

        private readonly IStatValueGiver _statValueGiver;

        public bool JumpActive { get { return _rigidbody.velocity.y > 0; } }
        public bool FallActive { get { return _rigidbody.velocity.y < 0; } }


        public Jumper(Rigidbody2D rigidbody, JumpData jumpData, IStatValueGiver startValueGiver)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;

            _statValueGiver = startValueGiver;
        }

        public void Jump(bool isCanJump)
        {
            if (!isCanJump)
                return;

            _rigidbody.AddForce(Vector2.up * _statValueGiver.GetStatValue(StatType.JumpForce));
            _rigidbody.gravityScale = _jumpData.GravityScale;
        }
    }
}
