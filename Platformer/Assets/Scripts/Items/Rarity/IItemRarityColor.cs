using UnityEngine;

using Items.Enum;

namespace Items.Rarity
{
    public interface IItemRarityColor
    {
        ItemRarity ItemRarity { get; }
        Color Color { get; }
    }
}