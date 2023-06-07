using System.Collections.Generic;
using UnityEngine;

namespace LevelSystem.Storage
{
    [CreateAssetMenu(fileName = "LevelStorage", menuName = "Level/LevelStorage")]
    public class LevelStorage: ScriptableObject
    {
        [field: SerializeField] public List<EnemiesSpawnData> ListEnemiesSpawnData { get; private set; }

        [field: SerializeField] public List<ItemsSpawnData> ListItemsSpawnData { get; private set; }
    }
}
