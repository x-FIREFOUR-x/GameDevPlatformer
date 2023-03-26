using System;
using UnityEngine;

using StatsSystem.Enum;

namespace StatsSystem
{
    [Serializable]
    public class StatModificator
    {
        [field: SerializeField] public Stat Stat { get; private set; }
        [field: SerializeField] public StatModificatorType StatModificatorType { get; private set; }

        [field: SerializeField] public float Duration { get; private set; }

        public float StartTime { get; }

        public StatModificator(Stat stat, StatModificatorType statModificatorType, float duration, float startTime)
        {
            Stat = stat;
            StatModificatorType = statModificatorType;
            Duration = duration;
            this.StartTime = startTime;
        }
    }
}
