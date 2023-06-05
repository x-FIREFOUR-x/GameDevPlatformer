using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Items.Core;
using Items.Enum;

namespace Items
{
    public class Inventory
    {
        public const int BackPackMaxSize = 24;
        public const int MaxCountEquipmentPotion = 2;

        private readonly Transform _player;
        
        public List<Item> BackPackItems { get; }
        public List<Equipment> EquipmentItems { get; }

        public event Action BackPackChanged;
        public event Action EquipmentChanged;
        public event Action<Item, Vector2> ItemDropped;

        public Inventory(List<Item> backPackItems, List<Equipment> equipmentList, Transform player)
        {
            _player = player;
            EquipmentItems = equipmentList ?? new List<Equipment>();

            if (backPackItems != null)
                return;

            BackPackItems = new List<Item>();
            for (int i = 0; i < BackPackMaxSize; i++)
                BackPackItems.Add(null);
        }

        public bool IsFullBackPack()
        {
            return !BackPackItems.Any(item => item == null);
        }

        public bool CanAddItemToStackExistItem(Item item)
        {
            if (item.Descriptor.Type != ItemType.Potion)
                return false;

            return BackPackItems.Find(element => element?.Descriptor.ItemId == item.Descriptor.ItemId) != null 
                || EquipmentItems.Find(element => element?.Descriptor.ItemId == item.Descriptor.ItemId) != null;
        }

        public void AddItemToInventory(Item item)
        {
            if (TryAddItemToStackExistItem(item))
                return;

            var index = BackPackItems.FindIndex(element => element == null);

            BackPackItems[index] = item;
            BackPackChanged?.Invoke();
            
        }

        public void RemoveItemFromBackPack(Item item, bool toWorld) 
        {
            BackPackItems[BackPackItems.IndexOf(item)] = null;
            BackPackChanged?.Invoke();

            if (toWorld)
                ItemDropped?.Invoke(item, _player.position);
        }

        public void Equip(Equipment equipment)
        {
            EquipmentItems.Add(equipment);
            if (equipment.EquipmentType != EquipmentType.Potion)
                equipment.Use();
            EquipmentChanged?.Invoke();
        }
        
        public void UnEquip(Equipment equipment, bool toWorld)
        {
            EquipmentItems.Remove(equipment);
            if (equipment.EquipmentType != EquipmentType.Potion)
                equipment.Use();
            EquipmentChanged?.Invoke();

            if (toWorld)
                ItemDropped?.Invoke(equipment, _player.position);
        }

        public void UsePotion(Equipment equipment)
        {
            if(equipment.EquipmentType == EquipmentType.Potion)
            {
                equipment.UsePotion();
                if (equipment.Amount == 0)
                    EquipmentItems.Remove(equipment);
                EquipmentChanged?.Invoke();
            }
        }

        private bool TryAddItemToStackExistItem(Item item)
        {
            if (item.Descriptor.Type != ItemType.Potion)
                return false;

            Item existItem = BackPackItems.Find(element => element?.Descriptor.ItemId == item.Descriptor.ItemId);
            existItem ??= EquipmentItems.Find(element => element?.Descriptor.ItemId == item.Descriptor.ItemId);

            if (existItem == null)
                return false;

            ((Potion)existItem).AddToStack(1);
            BackPackChanged?.Invoke();
            return true;
        }
    }
}
