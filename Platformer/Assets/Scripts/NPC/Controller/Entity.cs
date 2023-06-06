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
            StatsController.Dispose();
            _entityBehaviour.DamageTaken -= OnDamageTaken;
            _entityBehaviour.PlayDeath();
        }

        protected abstract void VisualiseHP(float currentHp, float maxHp);
        
        private void OnDamageTaken(float damage)
        {
            float defence = StatsController.GetStatValue(StatType.Defence);
            float damageThroughDefence = damage - defence;
            
            if (damageThroughDefence <= 0)
                return;

            float currentHp = StatsController.GetStatValue(StatType.Health);
            float newHpValue = Mathf.Clamp(currentHp - damageThroughDefence, 0, StatsController.GetStatValue(StatType.MaxHealth)); 
            
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
