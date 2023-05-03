using Items.Data;
using StatsSystem;

namespace Items.Core
{
    public abstract class Item
    {
        protected StatsController _statsController;
        protected StatChangingItemDescriptor _itemDescriptor;
        public ItemDescriptor Descriptor { get; }
        public abstract int Amount { get; }
        protected Item(ItemDescriptor descriptor) => Descriptor = descriptor;
        public abstract void Use();
    }
}