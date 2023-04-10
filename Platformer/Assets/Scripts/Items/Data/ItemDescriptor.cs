using System;
using Items.Enum;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class ItemDescriptor
    {
        public ItemId ItemId { get; private set; }
        public ItemType Type { get; private set; }
        public Sprite ItemSprite { get; private set; }
        public ItemRarity ItemRarity { get; private set; }
        public float Price { get; private set;  }

        public ItemDescriptor(ItemId itemId, ItemType type, Sprite itemSprite, ItemRarity itemRarity, float price)
        {
            ItemId = itemId;
            Type = type;
            ItemSprite = itemSprite;
            ItemRarity = itemRarity;
            Price = price;
        }
    }
}

