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
using NPC.Enum;
using NPC.Spawn;
using StatsSystem;
using UI;
using UnityEngine.Serialization;

namespace Core
{
    class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntityBehaviour _playerEntityBehaviour;
        [SerializeField] private GameUIInputView _ganeUIInputView;
        [SerializeField] private ItemRarityDescriptorsStorage _rarityDescriptorsStorage;
        [SerializeField] private LayerMask _whatIsPlayer;
        [SerializeField] private ItemStorage _itemsStorage;
        [SerializeField] private StatsStorage _statsStorage;
        [SerializeField] private Transform _pointOfSpawn;

        private ExternalDevicesInputReader _externalDevicesInput;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;
        private DropGenerator _dropGenerator;
        private ItemsSystem _itemsSystem;
        private UIContext _uiContext;
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

            _playerSystem = new PlayerSystem(_playerEntityBehaviour, new List<IEntityInputSource>
            {
                _ganeUIInputView,
                _externalDevicesInput
            });
            _disposables.Add(_playerSystem);

            ItemsFactory itemsFactory = new ItemsFactory(_playerSystem.StatsController);
            List<IItemRarityColor> rarityColors = _rarityDescriptorsStorage.RarityDescriptors.Cast<IItemRarityColor>().ToList();
            List<ItemDescriptor> descriptors = _itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _itemsSystem = new ItemsSystem(rarityColors, _whatIsPlayer, itemsFactory, _playerSystem.Inventory);
            _dropGenerator = new DropGenerator(descriptors, _playerEntityBehaviour, _itemsSystem);

            UIContext.Data data = new UIContext.Data(_playerSystem.Inventory, _rarityDescriptorsStorage.RarityDescriptors, _playerSystem.StatsController);
            _uiContext = new UIContext(new List<IWindowsInputSource> { _ganeUIInputView, _externalDevicesInput }, data);

            _entitySpawner = new EntitySpawner();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
            
            if (Input.GetKeyDown(KeyCode.M))
                _entitySpawner.SpawnEntity(EntityId.HeavyBandit, _pointOfSpawn.position);

            if (Input.GetKeyDown(KeyCode.N))
                _entitySpawner.SpawnEntity(EntityId.LightBandit, _pointOfSpawn.position);
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
    }
}
