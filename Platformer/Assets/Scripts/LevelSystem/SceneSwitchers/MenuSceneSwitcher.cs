using LevelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LevelSystem.SceneSwitchers
{
    class MenuSceneSwitcher : MonoBehaviour
    {
        [SerializeField] Button button;


        private void Awake()
        {
            button.onClick.AddListener(() => StartGame());
        }

        public void StartGame()
        {
            SceneController.Instance.NextLevelGameScene();
        }
    }
}
