using Items.Enum;
using UnityEngine;

namespace UI.InventoryUI.Element
{
    public class EquipmentSlot : ItemSlot
    {
        [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    }
}