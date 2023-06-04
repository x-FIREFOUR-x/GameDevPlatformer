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

        private float _currentHp;
        
        public event Action<Entity> Died;
        
        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
            _currentHp = statsController.GetStatValue(StatType.Health);
            _entityBehaviour.DamageTaken += OnDamageTaken;
        }
        
        public virtual void Dispose()
        {
            StatsController.Dispose();
            _entityBehaviour.DamageTaken -= OnDamageTaken;
        }

        protected abstract void VisualiseHP(float currentHp);
        
        private void OnDamageTaken(float damage)
        {
            // TODO: check for correctness after adding processing stats of equipped items 
            float defence = StatsController.GetStatValue(StatType.Defence);
            float damageThroughDefence = damage - defence;
            
            if (damageThroughDefence <= 0)
                return;
            
            _currentHp = Mathf.Clamp(_currentHp - damageThroughDefence, 0, _currentHp); 
            VisualiseHP(_currentHp);
        }
    }
}
