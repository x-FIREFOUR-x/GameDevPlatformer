using System.Collections.Generic;
using UnityEngine;

using StatsSystem;
using NPC.Behaviour;
using NPC.Enum;

namespace NPC.Data
{
    [CreateAssetMenu(fileName = nameof(EntityStorage), menuName = ("EntitiesSpawner/EntityStorage"))]
    public class EntityStorage : ScriptableObject
    {
        [field: SerializeField] public EntityId Id { get; private set; }
        [field: SerializeField] public List<Stat> Stats { get; private set; }
        [field: SerializeField] public BaseEntityBehaviour EntityBehaviourPrefab { get; private set; }
    }
}
