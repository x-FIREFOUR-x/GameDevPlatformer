using System;
using System.Collections.Generic;

using Items.Core;
using Items.Enum;

namespace Items
{
    public class EquipmentConditionChecker
    {
        public bool IsEquipmentConditionFits(Equipment equipment, List<Equipment> currentEquipment)
        {
            // will be updated at next epics
            return true;
        }

        public bool TryReplaceEquipment(Equipment equipment, out Equipment oldEquipment,
            List<Equipment> currentEquipments)
        {
            oldEquipment = currentEquipments.Find(slot => slot.EquipmentType == equipment.EquipmentType);

            switch (equipment.EquipmentType)
            {
                case EquipmentType.Helmet:
                case EquipmentType.Chest:
                case EquipmentType.Shield:
                case EquipmentType.Weapon:
                    return true;
                case EquipmentType.None:
                default:
                    throw new NullReferenceException($"Equipment with type {equipment.EquipmentType} is not handled!");
            }
        }
    }
}
