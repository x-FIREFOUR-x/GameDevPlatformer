using System;

namespace Fight
{
    public interface IDamageable
    {
        event Action<float> DamageTaken;
        void TakeDamage(float damage);
    }
}