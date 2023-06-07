using System;
using System.Collections.Generic;
using UnityEngine;

using Items.Enum;
using StatsSystem;

namespace Items.Data
{
    [Serializable]
    public class StatChangingItemDescriptor : ItemDescriptor
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public List<StatModificator> Stats { get; private set; }

        public StatChangingItemDescriptor(ItemId itemId, ItemType type, Sprite itemSprite, ItemRarity itemRarity, float price, int level, List<StatModificator> stats):
            base(itemId, type, itemSprite, itemRarity, price)
        {
            Level = level;
            Stats = stats;
        }
    }
}