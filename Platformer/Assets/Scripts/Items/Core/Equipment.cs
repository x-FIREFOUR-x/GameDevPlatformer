using Items.Data;
using Items.Enum;
using StatsSystem;

namespace Items.Core
{
    public class Equipment : Item
    {
        private bool _equipped;

        protected int _amount = -1;
        public override int Amount => _amount;
        
        public EquipmentType EquipmentType { get; }

        public Equipment(ItemDescriptor descriptor, StatsController statsController, EquipmentType equipmentType) :
            base(descriptor)
        {
            _itemDescriptor = descriptor as StatChangingItemDescriptor;
            _statsController = statsController;
            EquipmentType = equipmentType;
        }

        public override void Use()
        {
            if (_equipped)
            {
                UnEquip();
                return;
            }

            Equip();
        }

        private void Equip()
        {
            _equipped = true;
            foreach (var stat in _itemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat);
            }
        }

        private void UnEquip()
        {
            _equipped = false;
            foreach (var stat in _itemDescriptor.Stats)
            {
                _statsController.ProcessModificator(stat.GetReverseModificator());
            }
        }
    }
}