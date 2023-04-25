using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Core;
using Items.Data;
using Items.Enum;
using UI.Core;
using UI.InventoryUI.Element;
using UnityEngine;

namespace UI.InventoryUI
{
    public class InventoryScreenPresenter : ScreenController<InventoryScreenView>
    {
        private readonly Inventory _inventory;
        private readonly List<RarityDescriptor> _rarityDescriptors;

        private readonly Dictionary<ItemSlot, Item> _backPackSlots;
        private readonly Dictionary<EquipmentSlot, Equipment> _equipmentSlots;

        private readonly EquipmentConditionChecker _equipmentConditionChecker;

        private readonly Sprite _emptyBackSprite;

        public InventoryScreenPresenter(InventoryScreenView view, Inventory inventory, List<RarityDescriptor> rarityDescriptors) : base(view)
        {
            _inventory = inventory;

            _rarityDescriptors = rarityDescriptors;
            _emptyBackSprite = GetBackSprite(ItemRarity.None);
            _backPackSlots = new Dictionary<ItemSlot, Item>();
            _equipmentSlots = new Dictionary<EquipmentSlot, Equipment>();
            _equipmentConditionChecker = new EquipmentConditionChecker();
        }

        public override void Initialize()
        {
            View.MovingImage.gameObject.SetActive(false);
            InitializeBackPack();
            InitializeEquipment();
            _inventory.BackPackChanged += UpdateBackPack;
            _inventory.EquipmentChanged += UpdateEquipment;
            base.Initialize();
        }

        public override void Complete()
        {
            View.Hide();
            ClearBackPack();
            ClearEquipment();
            _inventory.BackPackChanged -= UpdateBackPack;
            _inventory.EquipmentChanged -= UpdateEquipment;
        }
        
        private void InitializeBackPack()
        {
            var backPack = View.BackPackSlots;
            for (int i = 0; i < backPack.Count; i++)
            {
                var slot = backPack[i];
                var item = _inventory.BackPackItems[i];
                _backPackSlots.Add(slot, item);

                if (item == null)
                    continue;
                
                slot.SetItem(item.Descriptor.ItemSprite,GetBackSprite(item.Descriptor.ItemRarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
        }
        
        private void InitializeEquipment()
        {
            var equipment = View.EquipmentSlots;
            foreach (EquipmentSlot slot in equipment)
            {
                var item = _inventory.EquipmentItems.Find(equip => equip.EquipmentType == slot.EquipmentType);
                _equipmentSlots.Add(slot,item);

                if (item == null)
                    continue;
                slot.SetItem(item.Descriptor.ItemSprite, GetBackSprite(item.Descriptor.ItemRarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
        }
        
        private void UseSlot(ItemSlot slot)
        {
            throw new System.NotImplementedException();
        }

        private void ClearSlot(ItemSlot obj)
        {
            throw new System.NotImplementedException();
        }
        
        private void UpdateBackPack()
        {
            ClearBackPack();
            InitializeBackPack();
        }
        
        private void UpdateEquipment()
        {
            ClearEquipment();
            InitializeEquipment();
        }
        
        private void ClearBackPack()
        {
            ClearSlots(_backPackSlots.Select(item => item.Key).ToList());
            _backPackSlots.Clear();
        }
        
        private void ClearEquipment()
        {
            ClearSlots(_equipmentSlots.Select(item => item.Key).Cast<ItemSlot>().ToList());
            _equipmentSlots.Clear();
        } 

        private void ClearSlots(List<ItemSlot> slotsToClear)
        {
            foreach (var slot in slotsToClear)
            {
                UnsubscribeSlotEvents(slot);
                slot.ClearItem(_emptyBackSprite);
            }
        }
        
        private void SubscribeToSlotEvents(ItemSlot slot)
        {
            slot.SlotClicked += UseSlot;
            slot.SlotClearClicked += ClearSlot;
        }
        
        private void UnsubscribeSlotEvents(ItemSlot slot)
        {
            slot.SlotClicked -= UseSlot;
            slot.SlotClearClicked -= ClearSlot;
        }

        private Sprite GetBackSprite(ItemRarity rarity) =>
            _rarityDescriptors.Find(descriptor => descriptor.ItemRarity == rarity).Sprite;
    }
}