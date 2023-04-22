using System;

namespace InputReader
{
    interface IWindowsInputSource
    {
        event Action InventoryRequested;
    }
}
