using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using Items;
using UI;
using InputReader;
using Items.Data;
using Items.Rarity;
using StatsSystem;
using Items.Storage;

namespace LevelSystem
{
    class SceneController: MonoBehaviour
    {
        public static SceneController Instance;

        public StatsController StatsController { get; set; }
        [field: SerializeField] public ItemRarityDescriptorsStorage RarityDescriptorsStorage { get; private set; }
        [field: SerializeField] private ItemStorage _itemsStorage;
        [field: SerializeField] private LayerMask _whatIsPlayer;

        public List<Stat> Stats { get; private set; }
        public DropGenerator DropGenerator;
        public Inventory Inventory { get; private set; }
        public ItemsSystem ItemsSystem;

        public UIContext UIContext { get; private set; }

        public ProjectUpdater ProjectUpdater;

        [SerializeField]
        private string _menuScene = "MenuScene";
        [SerializeField]
        private string _endScene = "EndScene";
        [SerializeField]
        private string _victoryScene = "VictoryScene";
        [SerializeField]
        private List<string> _Level1Scenes = new List<string>{ "Level1Scene", "Level2Scene" };
        private int _nextLevel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                UIContext = new UIContext();
            }
            else
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);

            ResetDate();
        }

        public void MenuGameScene()
        {
            ResetDate();
            SceneManager.LoadScene(_menuScene);
        }

        public void NextLevelGameScene()
        {
            if (_nextLevel < _Level1Scenes.Count)
            {
                SceneManager.LoadScene(_Level1Scenes[_nextLevel]);
                _nextLevel++;
            }
            else
            {
                SceneManager.LoadScene(_victoryScene);
            }
        }

        public void EndGameScene()
        {
            SceneManager.LoadScene(_endScene);
        }

        private void ResetDate()
        {
            _nextLevel = 0;

            if (Inventory == null)
                Inventory = new Inventory(null, null, null);
            else
                Inventory.Clear();

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            Stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();

            if (StatsController == null)
                StatsController = new StatsController(Stats);
            else
                StatsController.ResetStats(Stats);

            ItemsFactory itemsFactory = new ItemsFactory(StatsController);
            List<IItemRarityColor> rarityColors = RarityDescriptorsStorage.RarityDescriptors.Cast<IItemRarityColor>().ToList();
            List<StatChangingItemDescriptor> descriptors =
                _itemsStorage.ItemScriptables.Select(scriptable => (StatChangingItemDescriptor)scriptable.ItemDescriptor).ToList();

            if (ItemsSystem != null)
                ItemsSystem.Dispose();
            ItemsSystem = new ItemsSystem(rarityColors, _whatIsPlayer, itemsFactory, Inventory);
            DropGenerator = new DropGenerator(descriptors, ItemsSystem);

            if (ProjectUpdater.Instance == null)
                ProjectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            else
                ProjectUpdater = ProjectUpdater.Instance as ProjectUpdater;
        }
    }
}
