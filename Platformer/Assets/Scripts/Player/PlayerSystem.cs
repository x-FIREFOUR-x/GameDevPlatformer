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
        private readonly PlayerEntity _playerEntity;
        private readonly PlayerBrain _playerBrain;
        private readonly List<IDisposable> _disposables;
        
        public StatsController StatsController { get; }
        public Inventory Inventory { get;  }

        public PlayerSystem(PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);
            _disposables.Add(StatsController);

            _playerEntity = playerEntity;
            _playerEntity.Initialize(StatsController);

            _playerBrain = new PlayerBrain(_playerEntity, inputSources);
            _disposables.Add(_playerBrain);

            Inventory = new Inventory(null, null, _playerEntity.transform);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}
