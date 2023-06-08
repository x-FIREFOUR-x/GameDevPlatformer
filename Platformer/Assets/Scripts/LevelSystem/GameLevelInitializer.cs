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
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemsSystem _itemsSystem;
        private EntitySpawner _entitySpawner;

        private List<IDisposable> _disposables;

        private void Awake()
        {
            _disposables = new List<IDisposable>();
            if (ProjectUpdater.Instance == null)
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            else
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;


            _externalDevicesInput = new ExternalDevicesInputReader();
            _disposables.Add(_externalDevicesInput);

            SceneController.Instance.StatsController = new StatsController(SceneController.Instance.Stats);
            SceneController.Instance.Inventory.SetPlayer(_playerEntityBehaviour.transform);

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

            _entitySpawner = new EntitySpawner(_dropGenerator);

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
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
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
            _projectUpdater.IsPaused = false;
        }

        private void InitializeSystem()
        {
            ItemsFactory itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            List<IItemRarityColor> rarityColors = SceneController.Instance.RarityDescriptorsStorage.RarityDescriptors.Cast<IItemRarityColor>().ToList();
            List<StatChangingItemDescriptor> descriptors =
                _itemsStorage.ItemScriptables.Select(scriptable => (StatChangingItemDescriptor)scriptable.ItemDescriptor).ToList();
            _itemsSystem = new ItemsSystem(rarityColors, _whatIsPlayer, itemsFactory, _playerSystem.Inventory);
            _dropGenerator = new DropGenerator(descriptors, _playerEntityBehaviour, _itemsSystem);

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
                _itemsSystem.DropItem((StatChangingItemDescriptor)item.ItemDescriptor, itemsSpawnData.СoordinateSpawn);
            }
        }
    }
}
