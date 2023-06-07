using UnityEngine;

namespace LevelSystem
{
    class SceneSwitcher : MonoBehaviour
    {
        public void StartGame()
        {
            SceneController.Instance.NextLevelGameScene();
        }

        public void NextLevel()
        {
            SceneController.Instance.NextLevelGameScene();
        }

        public void EndGame()
        {
            SceneController.Instance.EndGameScene();
        }

        public void MenuGame()
        {
            SceneController.Instance.MenuGameScene();
        }
    }
}
