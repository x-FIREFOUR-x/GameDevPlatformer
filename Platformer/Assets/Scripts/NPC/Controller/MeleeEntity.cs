using UnityEngine;
using Pathfinding;

using StatsSystem;
using StatsSystem.Enum;
using NPC.Behaviour;

namespace NPC.Controller
{
    public class MeleeEntity : Entity
    {
        private readonly BaseEntityBehaviour _meleeEntityBehaviour;
        private readonly Seeker _pathSeeker;
        private readonly Vector2 _moveDelta;

        public MeleeEntity(BaseEntityBehaviour entityBehaviour, StatsController statsController) :
            base(entityBehaviour, statsController)
        {
            _pathSeeker = entityBehaviour.GetComponent<Seeker>();
            _meleeEntityBehaviour = entityBehaviour;
            
            var speedDelta = StatsController.GetStatValue(StatType.Speed) * Time.fixedDeltaTime;
            _moveDelta = new Vector2(speedDelta, 0);
        }
    }
}
