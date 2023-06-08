using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Player;
using InputReader;
using Core.Services.Updater;
using Items;
using Items.Data;
using Items.Rarity;
using Items.Storage;
using NPC.Spawn;
using StatsSystem;
using UI;
using LevelSystem.Storage;

namespace LevelSystem
{
    class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntityBehaviour _playerEntityBehaviour;
        [SerializeField] private LayerMask _whatIsPlayer;

        [SerializeField] private GameUIInputView _ganeUIInputView;
        private ExternalDevicesInputReader _externalDevicesInput;

        [Header("FinisArea")]
        [SerializeField] private Transform _finishArea;

        [Header("Storages")]
        [SerializeField] private ItemStorage _itemsStorage;
        [SerializeField] private StatsStorage _statsStorage;
        [SerializeField] private LevelStorage _levelStorage;

        private PlayerSystem _playerSystem;
        //private ProjectUpdater _projectUpdater;
        //private DropGenerator _dropGenerator;
        //private ItemsSystem _itemsSystem;
        private EntitySpawner _entitySpawner;

        private List<IDisposable> _disposables;

        private void Awake()
        {
            _disposables = new List<IDisposable>();

            _externalDevicesInput = new ExternalDevicesInputReader();
            _disposables.Add(_externalDevicesInput);

            //SceneController.Instance.StatsController = new StatsController(SceneController.Instance.Stats);
            SceneController.Instance.DropGenerator.SetPlayer(_playerEntityBehaviour);

            _playerSystem = new PlayerSystem
            (
                _playerEntityBehaviour,
                SceneController.Instance.Inventory,
                SceneController.Instance.StatsController,
                new List<IEntityInputSource>
                {
                    _ganeUIInputView,
                    _externalDevicesInput
                }
            );
            _disposables.Add(_playerSystem);

            InitializeSystem();

            _entitySpawner = new EntitySpawner(SceneController.Instance.DropGenerator);

            SpawnItemsAndEntities();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_finishArea.position, _finishArea.localScale);
            Gizmos.color = Color.red;
        }

        private void Update()
        {
            if(_playerEntityBehaviour != null)
            {
                var overlap = Physics2D.OverlapCircle(_finishArea.transform.position, 1, _whatIsPlayer);

                if(overlap != null)// && !_entitySpawner.IsEntities())
                {
                    SceneController.Instance.NextLevelGameScene();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                SceneController.Instance.ProjectUpdater.IsPaused = !SceneController.Instance.ProjectUpdater.IsPaused;
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        public void StartLevel()
        {
            SceneController.Instance.ProjectUpdater.IsPaused = false;
        }

        private void InitializeSystem()
        {
            SceneController.Instance.Inventory.SetPlayer(_playerEntityBehaviour.transform);

            UIContext.Data data = new UIContext.Data(
                SceneController.Instance.Inventory,
                SceneController.Instance.RarityDescriptorsStorage.RarityDescriptors,
                SceneController.Instance.StatsController);
            SceneController.Instance.UIContext.SetData(new List<IWindowsInputSource> { _ganeUIInputView, _externalDevicesInput }, data);
        }

        private void SpawnItemsAndEntities()
        {
            foreach (var enemiesSpawnData in _levelStorage.ListEnemiesSpawnData)
            {
                _entitySpawner.SpawnEntity(enemiesSpawnData.TypeEntity, enemiesSpawnData.СoordinateSpawn, enemiesSpawnData.LevelDropedItem);
            }
            foreach (var itemsSpawnData in _levelStorage.ListItemsSpawnData)
            {
                var item = _itemsStorage.ItemScriptables.Find(item => item.ItemDescriptor.ItemId == itemsSpawnData.IdItem);
                SceneController.Instance.ItemsSystem.DropItem((StatChangingItemDescriptor)item.ItemDescriptor, itemsSpawnData.СoordinateSpawn);
            }
        }
    }
}
