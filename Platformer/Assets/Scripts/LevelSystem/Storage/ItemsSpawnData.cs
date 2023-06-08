using System;
using UnityEngine;

using Items.Enum;

namespace LevelSystem.Storage
{
    [Serializable]
    public class ItemsSpawnData
    {
        [field: SerializeField] public ItemId IdItem { get; private set; }
        [field: SerializeField] public Vector3 СoordinateSpawn { get; private set; }
    }
}
