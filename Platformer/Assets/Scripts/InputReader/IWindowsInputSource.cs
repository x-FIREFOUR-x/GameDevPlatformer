using System;

namespace InputReader
{
    public interface IWindowsInputSource
    {
        event Action InventoryRequested;
    }
}
