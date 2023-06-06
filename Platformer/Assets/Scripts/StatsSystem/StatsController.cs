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
        public List<Stat> CurrentStats { get; }
        private readonly List<StatModificator> _activeModificators;

        public Action StatsChanges;

        private float precision = 0.01f;

        public StatsController(List<Stat> currentStats)
        {
            CurrentStats = currentStats;

            _activeModificators = new List<StatModificator>();
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public float GetStatValue(StatType statType) =>
            CurrentStats.Find(stat => stat.Type == statType);

        public void ProcessModificator(StatModificator modificator)
        {
            var statToChange = CurrentStats.Find(stat => stat.Type == modificator.Stat.Type);

            if (statToChange == null)
                return;

            var newValue = modificator.StatModificatorType == StatModificatorType.Additive ?
                statToChange + modificator.Stat :
                statToChange * modificator.Stat;

            if (modificator.Stat.Type == StatType.Health)
            {
                newValue = Mathf.Clamp(newValue, 0, GetStatValue(StatType.MaxHealth)); 
            }

            newValue = Mathf.Round(newValue * (1 / precision)) * precision;
            statToChange.SetStatValue(newValue);

            
            if (modificator.Duration < 0)
            {
                StatsChanges?.Invoke();
                return;
            }
                
            var existModificator = _activeModificators.Find(m => m.Stat.Type == modificator.Stat.Type);
            if (existModificator != null)
            {
                RemoveModificator(existModificator);
            }
            
            var tempModificator = new StatModificator(modificator.Stat, modificator.StatModificatorType, modificator.Duration, Time.time);

            _activeModificators.Add(tempModificator);

            StatsChanges?.Invoke();
        }

        public void UpdateStat(StatType statType, float value)
        {
            value = Mathf.Round(value * (1 / precision)) * precision;

            var statToChange = CurrentStats.Find(stat => stat.Type == statType);

            if (statToChange == null)
                return;
            
            statToChange.SetStatValue(value);
            
            StatsChanges?.Invoke();
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if (_activeModificators.Count == 0)
                return;

            var expiredModificator = _activeModificators.Where(modificator => Time.time > modificator.StartTime + modificator.Duration);

            while (expiredModificator.Count() != 0)
                RemoveModificator(expiredModificator.First());
        }

        private void RemoveModificator(StatModificator modificator)
        {
            var statToChange = CurrentStats.Find(stat => stat.Type == modificator.Stat.Type);

            if (statToChange == null)
                return;

            var previousModificator = modificator.GetReverseModificator();

            var newValue = modificator.StatModificatorType == StatModificatorType.Additive ?
                statToChange + previousModificator.Stat :
                statToChange * previousModificator.Stat;

            newValue = Mathf.Round(newValue * (1 / precision)) * precision;

            statToChange.SetStatValue(newValue);

            _activeModificators.Remove(modificator);
            
            StatsChanges?.Invoke();
        }
    }
}
