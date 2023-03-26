using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using StatsSystem.Enum;
using Core.Services.Updater;

namespace StatsSystem
{
    public class StatsController: IDisposable, IStatValueGiver
    {
        private readonly List<Stat> _currentStats;
        private readonly List<StatModificator> _activeModificators;

        public StatsController(List<Stat> currentStats)
        {
            _currentStats = currentStats;

            _activeModificators = new List<StatModificator>();
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public float GetStatValue(StatType statType) =>
            _currentStats.Find(stat => stat.Type == statType);

        public void ProcessModificator(StatModificator modificator)
        {
            var statToChange = _currentStats.Find(stat => stat.Type == modificator.Stat.Type);

            if (statToChange == null)
                return;

            var addedValue = modificator.StatModificatorType == StatModificatorType.Additive ?
                statToChange + modificator.Stat :
                statToChange * modificator.Stat;

            statToChange.SetStatValue(statToChange + addedValue);
            if (modificator.Duration < 0)
                return;

            if(_activeModificators.Contains(modificator))
            {
                _activeModificators.Remove(modificator);
            }
            else
            {
                var addedStat = new Stat(modificator.Stat.Type, -addedValue);
                var tempModificator = new StatModificator(addedStat, StatModificatorType.Additive, modificator.Duration, Time.time);

                _activeModificators.Add(tempModificator);
            }
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if (_activeModificators.Count == 0)
                return;

            var expiredModificator = _activeModificators.Where(modificator => modificator.StartTime + modificator.Duration > Time.time);

            foreach (var modificator in expiredModificator)
                ProcessModificator(modificator);
                 
        }
    }
}
