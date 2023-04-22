using System;
using System.Collections.Generic;

using Items.Core;
using UnityEngine;

namespace Items
{
    public class Inventory
    {
        public const int InventorySize = 30;
        
        public List<Item> BackPackItems { get;  }
        public List<Equipment> EquipmentList { get; }

        public event Action BackPackChanged;
        public event Action EquipmentChanged;

        public Inventory(List<Item> backPackItems, List<Equipment> equipmentList)
        {
            EquipmentList = equipmentList ?? new List<Equipment>();
            BackPackItems = backPackItems ?? new List<Item>();
        }

        public void AddItemToBackPack(Item item)
        {
            BackPackItems.Add(item);
            BackPackChanged?.Invoke();
        }

        public void RemoveItemFromBackPack(Item item)
        {
            BackPackItems[BackPackItems.IndexOf(item)] = null;
            BackPackChanged?.Invoke();
        }

        public void Equip(Equipment equipment)
        {
            EquipmentList.Add(equipment);
            EquipmentChanged?.Invoke();
        }
        
        public void UnEquip(Equipment equipment)
        {
            EquipmentList.Remove(equipment);
            EquipmentChanged?.Invoke();
        }
    }
}
