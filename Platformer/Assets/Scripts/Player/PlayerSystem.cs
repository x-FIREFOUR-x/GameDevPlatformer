using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using InputReader;
using Items;
using NPC.Controller;
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

        public PlayerSystem(PlayerEntityBehaviour playerEntityBehaviour, Inventory inventory, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
            StatsController = new StatsController(stats);

            _playerEntityBehaviour = playerEntityBehaviour;
            _playerEntityBehaviour.Initialize();

            _playerEntity = new PlayerEntity(_playerEntityBehaviour, inputSources, StatsController);
            _disposables.Add(_playerEntity);

            Inventory = inventory;
            Inventory.SetPlayer(_playerEntityBehaviour.transform);

            _playerEntity.Died += OnPlayerDied;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void OnPlayerDied(Entity entity)
        {
            entity.Dispose();
        }
    }
}
