using System;
using System.Collections.Generic;

using Items.Core;
using UnityEngine;

namespace Items
{
    public class Inventory
    {
        public const int BackPackMaxSize = 16;

        private readonly Transform _player;
        
        public List<Item> BackPackItems { get;  }
        public List<Equipment> EquipmentItems { get; }

        public event Action BackPackChanged;
        public event Action EquipmentChanged;

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

        public bool TryAddItemToBackPack(Item item)
        {
            var index = BackPackItems.FindIndex(element => element == null);

            if (index > BackPackMaxSize || index < 0)
                return false;

            BackPackItems[index] = item;
            BackPackChanged?.Invoke();
            return true;
        }

        public void RemoveItemFromBackPack(Item item, bool toWorld) 
        {
            BackPackItems[BackPackItems.IndexOf(item)] = null;
            BackPackChanged?.Invoke();
            if(toWorld) Debug.Log("Item Dropped"); //temp
        }

        public void Equip(Equipment equipment)
        {
            EquipmentItems.Add(equipment);
            EquipmentChanged?.Invoke();
        }
        
        public void UnEquip(Equipment equipment, bool toWorld)
        {
            EquipmentItems.Remove(equipment);
            EquipmentChanged?.Invoke();
            if(toWorld) Debug.Log("Item Dropped"); //temp
        }
    }
}
