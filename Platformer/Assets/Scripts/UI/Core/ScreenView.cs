using UnityEngine;

namespace UI.Core
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        public virtual void Show() => _canvas.enabled = true;
        public virtual void Hide() => _canvas.enabled = false;
    }
}
