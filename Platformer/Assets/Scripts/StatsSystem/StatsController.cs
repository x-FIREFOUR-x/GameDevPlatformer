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

            var newValue = modificator.StatModificatorType == StatModificatorType.Additive ?
                statToChange + modificator.Stat :
                statToChange * modificator.Stat;

            statToChange.SetStatValue(newValue);
            if (modificator.Duration < 0)
                return;

            if (_activeModificators.Contains(modificator))
            {
                RemoveModificator(modificator);
            }
            
            var addedStat = new Stat(modificator.Stat.Type, -newValue);
            var tempModificator = new StatModificator(addedStat, StatModificatorType.Additive, modificator.Duration, Time.time);

            _activeModificators.Add(tempModificator);
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if (_activeModificators.Count == 0)
                return;

            var expiredModificator = _activeModificators.Where(modificator => modificator.StartTime + modificator.Duration > Time.time);


            Debug.Log(Time.time);

            while (expiredModificator.Count() != 0)
                RemoveModificator(expiredModificator.First());
                 
        }

        private void RemoveModificator(StatModificator modificator)
        {
            Debug.Log("Here");

            var statToChange = _currentStats.Find(stat => stat.Type == modificator.Stat.Type);

            if (statToChange == null)
                return;

            var previousModificator = modificator.GetReverseModificator();

            var newValue = modificator.StatModificatorType == StatModificatorType.Additive ?
                statToChange + previousModificator.Stat :
                statToChange * previousModificator.Stat;

            statToChange.SetStatValue(newValue);

            Debug.Log(previousModificator);

            _activeModificators.Remove(modificator);
        }
    }
}
