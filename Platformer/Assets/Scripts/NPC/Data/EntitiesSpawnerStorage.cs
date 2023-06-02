using System.Collections.Generic;
using UnityEngine;

namespace NPC.Data
{
    [CreateAssetMenu(fileName = nameof(EntitiesSpawnerStorage), menuName = ("EntitiesSpawner/EntitiesSpawnerStorage"))]
    public class EntitiesSpawnerStorage : ScriptableObject
    {
        [field: SerializeField] public List<EntityStorage> EntitiesData { get; private set; }
    }
}