using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

using Items;
using UI;
using InputReader;
using StatsSystem;
using Items.Storage;

namespace LevelSystem
{
    class SceneController: MonoBehaviour
    {
        public static SceneController Instance;

        public StatsController StatsController { get; set; }
        [field: SerializeField] public ItemRarityDescriptorsStorage RarityDescriptorsStorage { get; private set; }
        public List<Stat> Stats { get; private set; }

        public Inventory Inventory { get; private set;}
        public UIContext UIContext { get; private set; }

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

            _nextLevel = 0;
            Inventory = new Inventory(null, null, null);

            var statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            Stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToList();
        }

        public void MenuGameScene()
        {
            SceneManager.LoadScene(_menuScene);
        }

        public void NextLevelGameScene()
        {
            if (_nextLevel < _Level1Scenes.Count)
            {
                if(_nextLevel != 0)
                    SceneManager.LoadScene(_menuScene);
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
    }
}
