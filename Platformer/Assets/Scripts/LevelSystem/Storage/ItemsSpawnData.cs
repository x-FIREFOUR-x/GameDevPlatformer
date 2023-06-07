using System;
using UnityEngine;

using Items.Enum;
using Items.Scriptable;

namespace LevelSystem.Storage
{
    [Serializable]
    public class ItemsSpawnData
    {
        [field: SerializeField] public BaseItemScriptable ItemScriptable { get; private set; }
        [field: SerializeField] public Vector3 СoordinateSpawn { get; private set; }
    }
}
