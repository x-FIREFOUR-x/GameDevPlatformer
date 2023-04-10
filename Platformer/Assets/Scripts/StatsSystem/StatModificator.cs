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
            StartTime = startTime;
        }

        public StatModificator GetReverseModificator()
        {
            var reverseStat = new Stat(Stat.Type, StatModificatorType == StatModificatorType.Additive ? -Stat : 1 / Stat);
            return new StatModificator(reverseStat, StatModificatorType, Duration, 0);
        }
    }
}
