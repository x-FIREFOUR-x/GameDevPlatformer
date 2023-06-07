using System;
using System.Collections.Generic;
using UnityEngine;

using NPC.Controller;
using NPC.Data;
using NPC.Enum;
using Items;

namespace NPC.Spawn
{
    public class EntitySpawner : IDisposable
    {
        private readonly List<Entity> _spawnedEntities;
        private readonly EntitiesFactory _entitiesFactory;

        private readonly DropGenerator _dropGenerator;

        public EntitySpawner(DropGenerator dropGenerator)
        {
            _dropGenerator = dropGenerator;

            _spawnedEntities = new List<Entity>();
            
            var entitiesSpawnerStorage = Resources.Load<EntitiesSpawnerStorage>($"{nameof(EntitySpawner)}/{nameof(EntitiesSpawnerStorage)}");
            _entitiesFactory = new EntitiesFactory(entitiesSpawnerStorage);
        }
        
        public void SpawnEntity(EntityId entityId, Vector2 position, int levelDropedItem)
        {
            var entity = _entitiesFactory.GetEntityBrain(entityId, position, levelDropedItem);
            entity.Died += RemoveEntity;

            _spawnedEntities.Add(entity);
        }

        public void Dispose()
        {
            foreach (var entity in _spawnedEntities)
                entity.Died -= RemoveEntity;
            _spawnedEntities.Clear();
        }

        private void RemoveEntity(Entity entity)
        {
            entity.Died -= RemoveEntity;
            _spawnedEntities.Remove(entity);

            //((MeleeEntity)entity).LevelDropedItem
            
            entity.Dispose();
            entity = null;
        }
    }
}

