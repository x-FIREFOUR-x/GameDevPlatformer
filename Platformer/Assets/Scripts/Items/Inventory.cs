using System;
using System.Collections.Generic;

using Items.Core;
using UnityEngine;

namespace Items
{
    public class Inventory
    {
        public const int BackPackMaxSize = 30;
        
        public List<Item> BackPackItems { get;  }
        public List<Equipment> EquipmentItems { get; }

        public event Action BackPackChanged;
        public event Action EquipmentChanged;

        public Inventory(List<Item> backPackItems, List<Equipment> equipmentList)
        {
            EquipmentItems = equipmentList ?? new List<Equipment>();
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
            EquipmentItems.Add(equipment);
            EquipmentChanged?.Invoke();
        }
        
        public void UnEquip(Equipment equipment)
        {
            EquipmentItems.Remove(equipment);
            EquipmentChanged?.Invoke();
        }
    }
}
