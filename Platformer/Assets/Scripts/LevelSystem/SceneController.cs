using UnityEngine;
using UnityEngine.SceneManagement;

using Items;
using System.Collections.Generic;


namespace LevelSystem
{
    class SceneController: MonoBehaviour
    {
        public static SceneController Instance;

        public Inventory Inventory { get; private set;}

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
            }
            else
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);

            ResetData();
        }

        public void MenuGameScene()
        {
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
                ResetData();
                SceneManager.LoadScene(_victoryScene);
            }
        }

        public void EndGameScene()
        {
            ResetData();
            SceneManager.LoadScene(_endScene);
        }

        private void ResetData()
        {
            _nextLevel = 0;
            Inventory = new Inventory(null, null, null);
        }
    }
}
