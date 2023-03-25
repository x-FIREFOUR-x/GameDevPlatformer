using UnityEngine;

using Movement.Data;

namespace Movement.Controller
{
    public class Attacker
    {
        private readonly AttackData _attackData;

        private float _remainingTimeAttack;


        public bool AttackActive { get; private set; }


        public Attacker(AttackData attackData)
        {
            _attackData = attackData;
        }

        public void Attack(bool isCanAttack)
        {
            if (!isCanAttack)
                return;

            AttackActive = true;
            _remainingTimeAttack = _attackData.TimeAttack;
        }

        public void UpdateAtack()
        {
            if (AttackActive)
            {
                _remainingTimeAttack -= Time.deltaTime;

                if (_remainingTimeAttack <= 0)
                    AttackActive = false;
            }
        }
    }
}
