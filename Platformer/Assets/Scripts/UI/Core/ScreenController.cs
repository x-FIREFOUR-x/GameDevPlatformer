using System;
using UI.Enum;

namespace UI.Core
{
    public abstract class ScreenController<TView> : IScreenController where TView: ScreenView
    {
        protected TView View;

        public ScreenController(TView view) => View = view;

        public event Action<ScreenType> OpenScreenRequested;
        public event Action CloseScreenRequested;

        public virtual void Initialize() => View.Show();
        public virtual void Complete() => View.Hide();

        protected void RequestClose() => CloseScreenRequested?.Invoke();

        protected void RequestScreen(ScreenType characterScreenType) =>
            OpenScreenRequested?.Invoke(characterScreenType);
    }
}   