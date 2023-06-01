using System;

using NPC.Behaviour;
using StatsSystem;

namespace NPC.Controller
{
    public abstract class Entity
    {
        private readonly BaseEntityBehaviour _entityBehaviour;
        protected readonly StatsController StatsController;

        public event Action<Entity> Died;
        
        protected Entity(BaseEntityBehaviour entityBehaviour, StatsController statsController)
        {
            _entityBehaviour = entityBehaviour;
            _entityBehaviour.Initialize();
            StatsController = statsController;
        }
        
        public virtual void Dispose() => StatsController.Dispose();
        
    }
}
