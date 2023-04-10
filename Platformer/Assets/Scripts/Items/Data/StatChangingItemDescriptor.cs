using System;
using System.Collections.Generic;
using Items.Enum;
using StatsSystem;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class StatChangingItemDescriptor : ItemDescriptor
    {
        [field: SerializeField] public float Level { get; private set; }
        [field: SerializeField] public List<StatModificator> Stats { get; private set; }

        public StatChangingItemDescriptor(ItemId itemId, ItemType type, Sprite itemSprite, ItemRarity itemRarity, float price, float level, List<StatModificator> stats):
            base(itemId, type, itemSprite, itemRarity, price)
        {
            Level = level;
            Stats = stats;
        }
    }
}