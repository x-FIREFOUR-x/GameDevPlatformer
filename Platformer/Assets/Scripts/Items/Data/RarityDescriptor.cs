using System;
using UnityEngine;

using Items.Enum;
using Items.Rarity;

namespace Items.Data
{
    [Serializable]
    public class RarityDescriptor : IItemRarityColor
    {
        [field:SerializeField] public ItemRarity ItemRarity { get; private set; }
        [field:SerializeField] public Sprite Sprite { get; private set; }
        [field:SerializeField] public Color Color { get; private set; }
    }
}