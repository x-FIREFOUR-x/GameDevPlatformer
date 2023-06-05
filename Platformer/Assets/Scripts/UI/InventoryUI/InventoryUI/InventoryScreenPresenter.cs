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

namespace UI.InventoryUI.InventoryUI
{
    public class InventoryScreenPresenter : ScreenController<InventoryScreenView>, IDisposable
    {
        private readonly Inventory _inventory;
        private readonly List<RarityDescriptor> _rarityDescriptors;

        private readonly Dictionary<ItemSlot, Item> _backPackSlots;
        private readonly Dictionary<ItemSlot, Equipment> _equipmentSlots;

        private readonly EquipmentConditionChecker _equipmentConditionChecker;

        private readonly Sprite _emptyBackSprite;

        public InventoryScreenPresenter(InventoryScreenView view, Inventory inventory, List<RarityDescriptor> rarityDescriptors) : base(view)
        {
            _inventory = inventory;

            _rarityDescriptors = rarityDescriptors;
            _emptyBackSprite = GetBackSprite(ItemRarity.None);
            _backPackSlots = new Dictionary<ItemSlot, Item>();
            _equipmentSlots = new Dictionary<ItemSlot, Equipment>();
            _equipmentConditionChecker = new EquipmentConditionChecker();
            View.CloseClicked += RequestCloseScreen;
        }
        
        public void Dispose()
        {
            View.CloseClicked -= RequestCloseScreen;
        }
        
        public override void Initialize()
        {
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
        
        private void UseSlot(ItemSlot slot)
        {
            Equipment equipment;
            if (slot.EquipmentType != EquipmentType.None)
            {
                equipment = _equipmentSlots[slot];
                if (_inventory.IsFullBackPack())
                    return;

                _inventory.UnEquip(equipment, false);
                _inventory.AddItemToInventory(equipment);

                return;
            }

            Item item = _backPackSlots[slot];

            if (item is not Equipment equip)
                return;
            
            equipment = equip;

            if (!_equipmentConditionChecker.TryReplaceEquipment(equipment, out var prevEquipment,
                    _inventory.EquipmentItems, Inventory.MaxCountEquipmentPotion))
                return;
            
            
            _inventory.RemoveItemFromBackPack(equipment, false);
            if (prevEquipment != null)
            {
                if (_inventory.IsFullBackPack())
                    return;
                _inventory.UnEquip(prevEquipment, false);
                _inventory.AddItemToInventory((Item)prevEquipment);
            }
            
            _inventory.Equip(equipment);
        }

        private void ClearSlot(ItemSlot slot)
        {
            if(_backPackSlots.TryGetValue(slot, out Item item))
                _inventory.RemoveItemFromBackPack(item,true);
            
            if(slot.EquipmentType == EquipmentType.None && _equipmentSlots.TryGetValue(slot, out Equipment equipment))
                _inventory.UnEquip(equipment,true);
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
