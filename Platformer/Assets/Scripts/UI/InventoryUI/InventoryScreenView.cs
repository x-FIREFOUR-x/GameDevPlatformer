using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UI.Core;
using UI.InventoryUI.Element;

namespace UI.InventoryUI
{
    public class InventoryScreenView : ScreenView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _coinsText;
        
        [SerializeField] private Transform _backPackContainer;
        [SerializeField] private Transform _equipmentContainer;
        
        [field: SerializeField] public Image MovingImage { get; private set; }
        public List<ItemSlot> ItemSlots { get; private set; }
        public List<ItemSlot> EquipmentSlots { get; private set; }

        public event Action CloseClicked;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            ItemSlots = GetComponentsInChildren<ItemSlot>().ToList();
            EquipmentSlots = GetComponentsInChildren<EquipmentSlot>().ToList();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }

        public void SetCoinsAmount(string amount) => _coinsText.text = amount;
    }
}
