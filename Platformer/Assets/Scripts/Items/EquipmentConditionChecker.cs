using System;
using System.Collections.Generic;

using Items.Core;
using Items.Enum;

namespace Items
{
    public class EquipmentConditionChecker
    {
        public bool TryReplaceEquipment(Equipment equipment, out Equipment oldEquipment,
            List<Equipment> currentEquipments, int countPotion)
        {
            if (equipment.EquipmentType == EquipmentType.Potion)
            {
                int currentCountPotion = currentEquipments.FindAll(slot => slot.EquipmentType == equipment.EquipmentType).Count;

                if (currentCountPotion < countPotion)
                {
                    oldEquipment = null;
                    return true;
                }
            }

            oldEquipment = currentEquipments.Find(slot => slot.EquipmentType == equipment.EquipmentType);

            switch (equipment.EquipmentType)
            {
                case EquipmentType.Helmet:
                case EquipmentType.Chest:
                case EquipmentType.Shield:
                case EquipmentType.Weapon:
                case EquipmentType.Potion:
                    return true;
                case EquipmentType.None:
                default:
                    throw new NullReferenceException($"Equipment with type {equipment.EquipmentType} is not handled!");
            }
        }
    }
}
