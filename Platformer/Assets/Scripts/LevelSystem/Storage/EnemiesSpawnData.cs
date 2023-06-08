using System;
using UnityEngine;

using NPC.Enum;

namespace LevelSystem.Storage
{
    [Serializable]
    public class EnemiesSpawnData
    {
        [field: SerializeField] public EntityId TypeEntity { get; private set; }
        [field: SerializeField] public Vector3 СoordinateSpawn { get; private set; }
        [field: SerializeField] public int LevelDropedItem { get; private set; }
    }
}
