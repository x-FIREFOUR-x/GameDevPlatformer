using System;

using NPC.Behaviour;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace NPC.Controller
{
    public abstract class Entity
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;
        
        protected bool IsAttacking = false;

        public event Action<Entity> Died;
        
        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
            _entityBehaviour.DamageTaken += OnDamageTaken;
        }
        
        public virtual void Dispose()
        {
            _entityBehaviour.DamageTaken -= OnDamageTaken;
        }

        public void Death()
        {
            _entityBehaviour.PlayDeath();
            StatsController.Dispose();
        }
        public Vector3 GetCoordinate() => _entityBehaviour.transform.position;

        protected abstract void VisualiseHP(float currentHp, float maxHp);
        
        protected virtual void OnDamageTaken(float damage)
        {
            damage = damage * (1 - StatsController.GetStatValue(StatType.Defence));
            if (damage <= 0)
                return;

            damage = (float)Math.Round(damage);

            float currentHp = StatsController.GetStatValue(StatType.Health);
            float newHpValue = Mathf.Clamp(currentHp - damage, 0, StatsController.GetStatValue(StatType.MaxHealth));
            newHpValue = (float)Math.Round(newHpValue, 2);

            VisualiseHP(newHpValue, StatsController.GetStatValue(StatType.MaxHealth));
            StatsController.UpdateStat(StatType.Health, newHpValue);
            
            _entityBehaviour.PlayHurt();
            _entityBehaviour.EndAttack();

            if (newHpValue <= 0)
            {
                Died?.Invoke(this);
            }
        }
    }
}
