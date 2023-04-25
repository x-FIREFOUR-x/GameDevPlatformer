using System;
using Items.Enum;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Items.Enum;

namespace UI.InventoryUI.Element
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] protected Button ClearButton;
        
        [SerializeField] private Image _itemBackground;
        [SerializeField] private Image _emptyBackground;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _itemAmount;
        
        [SerializeField] private Button _slotButton;

        [field: SerializeField] public EquipmentType EquipmentType { get; private set; }

        public event Action<ItemSlot> SlotClearClicked;
        public event Action<ItemSlot> SlotClicked;

        private void Awake()
        {
            ClearButton.onClick.AddListener(() => SlotClearClicked?.Invoke(this));
            _slotButton.onClick.AddListener(() => SlotClicked?.Invoke(this));
        }

        public void SetItem(Sprite iconSprite, Sprite itemBackSprite, int amount)
        {
            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = iconSprite;
            
            _emptyBackground.gameObject.SetActive(false);
            _itemBackground.sprite = itemBackSprite;
            
            ClearButton.gameObject.SetActive(true);
            _itemAmount.gameObject.SetActive(amount > 0);
            _itemAmount.text = amount.ToString();
        }

        public void ClearItem(Sprite emptyBackSprite)
        {
            _itemBackground.sprite = emptyBackSprite;
            _itemIcon.gameObject.SetActive(false);
            
            _itemAmount.gameObject.SetActive(false);
            _emptyBackground.gameObject.SetActive(true);
            ClearButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ClearButton.onClick.RemoveAllListeners();
            _slotButton.onClick.RemoveAllListeners();
        }
    }
}

