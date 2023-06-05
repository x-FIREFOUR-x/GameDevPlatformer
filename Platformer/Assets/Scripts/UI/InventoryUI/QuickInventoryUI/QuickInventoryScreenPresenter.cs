using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Items;
using Items.Core;
using Items.Data;
using Items.Enum;
using UI.Core;
using UI.InventoryUI.Element;

namespace UI.InventoryUI.QuickInventoryUI
{
    class QuickInventoryScreenPresenter : ScreenController<QuickInventoryScreenView> , IDisposable
    {
        private readonly Inventory _inventory;
        private readonly List<RarityDescriptor> _rarityDescriptors;

        private readonly Dictionary<ItemSlot, Equipment> _equipmentSlots;

        private readonly Sprite _emptyBackSprite;

        public QuickInventoryScreenPresenter(QuickInventoryScreenView view, Inventory inventory, List<RarityDescriptor> rarityDescriptors) : base(view)
        {
            _inventory = inventory;

            _rarityDescriptors = rarityDescriptors;
            _emptyBackSprite = GetBackSprite(ItemRarity.None);
            _equipmentSlots = new Dictionary<ItemSlot, Equipment>();

            _inventory.EquipmentChanged += UpdateEquipment;
            _inventory.BackPackChanged += UpdateEquipment;
        }

        public void Dispose()
        {
            _inventory.EquipmentChanged -= UpdateEquipment;
            _inventory.BackPackChanged -= UpdateEquipment;
        }

        private void UpdateEquipment()
        {
            ClearEquipment();
            InitializeEquipment();
        }

        private void InitializeEquipment()
        {
            var equipment = View.EquipmentSlots;
            List<Equipment> setPotions = new List<Equipment>();
            foreach (ItemSlot slot in equipment)
            {
                Equipment item;
                if (slot.EquipmentType == EquipmentType.Potion)
                {
                    item = _inventory.EquipmentItems.Find(equip => equip.EquipmentType == slot.EquipmentType && setPotions.IndexOf(equip) == -1);
                    if (item != null)
                        setPotions.Add(item);
                }
                else
                {
                    item = _inventory.EquipmentItems.Find(equip => equip.EquipmentType == slot.EquipmentType);
                }
                _equipmentSlots.Add(slot, item);

                if (item == null)
                    continue;

                slot.SetItem(item.Descriptor.ItemSprite, GetBackSprite(item.Descriptor.ItemRarity), item.Amount);
                SubscribeToSlotEvents(slot);
            }
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
        }

        private void UnsubscribeSlotEvents(ItemSlot slot)
        {
            slot.SlotClicked -= UseSlot;
        }

        private void UseSlot(ItemSlot slot)
        {
                
            if(slot.EquipmentType == EquipmentType.Potion)
            {
                var equipment = _equipmentSlots[slot];
                _inventory.UsePotion(equipment);
            }
        }

        
        private Sprite GetBackSprite(ItemRarity rarity) =>
            _rarityDescriptors.Find(descriptor => descriptor.ItemRarity == rarity).Sprite;
    }
}
