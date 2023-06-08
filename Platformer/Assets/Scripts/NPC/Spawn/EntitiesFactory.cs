using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

using StatsSystem;
using NPC.Behaviour;
using NPC.Controller;
using NPC.Data;
using NPC.Enum;

namespace NPC.Spawn
{
    public class EntitiesFactory
    {
        private readonly Transform _entitiesContainer;
        private readonly EntitiesSpawnerStorage _entitiesSpawnerStorage;
        
        public EntitiesFactory(EntitiesSpawnerStorage entitiesSpawnerStorage)
        {
            _entitiesSpawnerStorage = entitiesSpawnerStorage;
            
            var gameObject = new GameObject
            {
                name = nameof(EntitySpawner)
            };
            
            _entitiesContainer = gameObject.transform;
        }

        public Entity GetEntityBrain(EntityId entityId, Vector2 position, int levelDropedItem)
        {
            var data = _entitiesSpawnerStorage.EntitiesData.Find(element => element.Id == entityId);

            if (data == null)
            {
                throw new InvalidOperationException("No entity in storage with this id!");
            }

            var entityBehaviour = Object.Instantiate(data.EntityBehaviourPrefab, position, data.EntityBehaviourPrefab.transform.rotation);
            entityBehaviour.transform.SetParent(_entitiesContainer);
            
            var stats = data.Stats.Select(stat => stat.GetCopy()).ToList();
            var statsController = new StatsController(stats);
            
            switch (entityId)
            {
                case EntityId.LightBandit:
                case EntityId.HeavyBandit:
                    return new MeleeEntity(entityBehaviour as MeleeEntityBehaviour, statsController, levelDropedItem);
                case EntityId.None:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
