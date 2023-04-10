using Items.Data;
using StatsSystem;
using UnityEngine;

namespace Items.Core
{
    public class Potion : Item
    {
        private readonly StatsController _statsController;
        private readonly StatChangingItemDescriptor _itemDescriptor;
        private int _amount;
        
        public override int Amount => -1;

        public Potion(ItemDescriptor descriptor, StatsController statsController) :
            base(descriptor)
        {
            _itemDescriptor = descriptor as StatChangingItemDescriptor;
            _statsController = statsController;
            _amount = 1;
        }

        public override void Use()
        {
            _amount--;
            foreach (var stat in _itemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }

            if (_amount <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            Debug.Log("Destroy");
        }

        public void AddToStack(int amount)
        {
            _amount += amount;
        }
    }
}