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

        protected bool IsAttacking = false;

        public event Action<Entity> Died;
        
        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
            _currentHp = statsController.GetStatValue(StatType.MaxHealth);
            _entityBehaviour.DamageTaken += OnDamageTaken;
        }
        
        public virtual void Dispose()
        {
            StatsController.Dispose();
            _entityBehaviour.DamageTaken -= OnDamageTaken;
            _entityBehaviour.PlayDeath();
        }

        protected abstract void VisualiseHP(float currentHp);
        
        protected virtual void OnDamageTaken(float damage)
        {
            damage = damage * (1 - StatsController.GetStatValue(StatType.Defence));
            
            if (damage <= 0)
                return;
            
            _currentHp = Mathf.Clamp(_currentHp - damage, 0, _currentHp); 
            VisualiseHP(_currentHp);
            _entityBehaviour.PlayHurt();

            _entityBehaviour.EndAttack();

            if (_currentHp <= 0)
            {
                Died?.Invoke(this);
            }
        }
    }
}
