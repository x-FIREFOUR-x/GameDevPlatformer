using System.Collections.Generic;
using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "LevelStorage", menuName = "Level/LevelStorage")]
    public class LevelStorage: ScriptableObject
    {
        [field: SerializeField] public List<EnemiesData> ListEnemiesData { get; private set; }
    }
}
