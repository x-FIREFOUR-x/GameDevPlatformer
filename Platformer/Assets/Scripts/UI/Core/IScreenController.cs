using System;

using UI.Enum;

namespace UI.Core
{
    public interface IScreenController
    {
        public event Action<ScreenType> OpenScreenRequested;
        public event Action CloseScreenRequested;

        void Initialize();

        void Complete();
    }
}
