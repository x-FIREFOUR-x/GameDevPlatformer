using LevelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LevelSystem.SceneSwitchers
{
    class FinishSceneSwitcher : MonoBehaviour
    {
        [SerializeField] Button button;


        private void Awake()
        {
            button.onClick.AddListener(() => MenuScene());
        }

        public void MenuScene()
        {
            SceneController.Instance.MenuGameScene();
        }
    }
}
