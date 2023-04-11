
using Items.Enum;
using UnityEngine;

namespace Items.Rarity
{
    public interface IItemRarityColor
    {
        ItemRarity ItemRarity { get; }
        Color Color { get; }
    }
}