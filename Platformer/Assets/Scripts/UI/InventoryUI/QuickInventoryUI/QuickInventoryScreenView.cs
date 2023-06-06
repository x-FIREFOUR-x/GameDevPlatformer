using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Items.Enum;
using UI.Core;
using UI.InventoryUI.Element;

namespace UI.InventoryUI.QuickInventoryUI
{
    class QuickInventoryScreenView : ScreenView
    {
        [SerializeField] private Transform _equipmentContainer;

        public List<ItemSlot> EquipmentSlots { get; private set; }

        private void Awake()
        {
            EquipmentSlots = GetComponentsInChildren<ItemSlot>().Where(slot => slot.EquipmentType != EquipmentType.None).ToList();
        }
    }
}
