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

        protected void RequestCloseScreen() => CloseScreenRequested?.Invoke();

        protected void RequestOpenScreen(ScreenType screenType) =>
            OpenScreenRequested?.Invoke(screenType);
    }
}   