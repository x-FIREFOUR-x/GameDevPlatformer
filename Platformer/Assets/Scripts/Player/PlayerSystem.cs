using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using InputReader;
using Items;
using StatsSystem;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly PlayerEntityBehaviour _playerEntityBehaviour;
        private readonly PlayerEntity _playerEntity;
        private readonly List<IDisposable> _disposables;
        
        public StatsController StatsController { get; }
        public Inventory Inventory { get;  }

        public PlayerSystem(PlayerEntityBehaviour playerEntityBehaviour, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);

            _playerEntityBehaviour = playerEntityBehaviour;
            _playerEntityBehaviour.Initialize();

            _playerEntity = new PlayerEntity(_playerEntityBehaviour, inputSources, StatsController);
            _disposables.Add(_playerEntity);

            Inventory = new Inventory(null, null, _playerEntityBehaviour.transform);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}
