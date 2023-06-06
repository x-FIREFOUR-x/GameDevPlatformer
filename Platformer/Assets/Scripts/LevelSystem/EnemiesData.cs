using System;
using UnityEngine;

using NPC.Enum;

namespace LevelSystem
{
    [Serializable]
    public class EnemiesData
    {
        [field: SerializeField] public EntityId TypeEntity { get; private set; }
        [field: SerializeField] public Vector3 coordinateSpawn { get; private set; }
    }
}
