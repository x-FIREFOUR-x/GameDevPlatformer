using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using InputReader;
using Items;
using NPC.Controller;
using StatsSystem;
using LevelSystem;

namespace Player
{
    public class PlayerSystem : IDisposable
    {
        private readonly PlayerEntityBehaviour _playerEntityBehaviour;
        private readonly PlayerEntity _playerEntity;
        private readonly List<IDisposable> _disposables;
        
        public StatsController StatsController { get; }
        public Inventory Inventory { get;  }

        public PlayerSystem(PlayerEntityBehaviour playerEntityBehaviour, Inventory inventory, StatsController statsController, List<IEntityInputSource> inputSources)
        {
            _disposables = new List<IDisposable>();

            StatsController = statsController;

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
            SceneController.Instance.EndGameScene();
        }
    }
}
