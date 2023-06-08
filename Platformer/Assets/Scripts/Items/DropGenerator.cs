using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Core.Services.Updater;
using Items.Data;
using Items.Enum;
using Player;

namespace Items
{
    public class DropGenerator
    {
        private PlayerEntityBehaviour _playerEntityBehaviour;
        private readonly List<StatChangingItemDescriptor> _itemDescriptors;
        private readonly ItemsSystem _itemsSystem;

        public DropGenerator(List<StatChangingItemDescriptor> itemDescriptors, ItemsSystem itemsSystem)
        {
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
        }

        public void SetPlayer(PlayerEntityBehaviour playerEntityBehaviour)
        {
            _playerEntityBehaviour = playerEntityBehaviour;
        }

        public void DropRandomItem(int level, Vector3 position, float chanceOfDrop = 1, float offsetHeight = 1.2f)
        {
            if (UnityEngine.Random.Range(0, 100) / 100.0f > chanceOfDrop)
                return;

            List<StatChangingItemDescriptor> items = _itemDescriptors.Where(item => item.Level == level || item.Level == 0).ToList();
            StatChangingItemDescriptor itemDescriptor = items[UnityEngine.Random.Range(0, items.Count)];

            position.y = position.y + offsetHeight;
            _itemsSystem.DropItem(itemDescriptor, position);
        }

        private void DropRandomItem(ItemRarity rarity)
        {
            List<StatChangingItemDescriptor> items = _itemDescriptors.Where(item => item.ItemRarity == rarity).ToList();
            StatChangingItemDescriptor itemDescriptor = items[UnityEngine.Random.Range(0, items.Count)];
            _itemsSystem.DropItem(itemDescriptor, _playerEntityBehaviour.transform.position + UnityEngine.Vector3.one);
        }

        private ItemRarity GetDropRarity()
        {
            float chance = Random.Range(0, 100);
            return chance switch
            {
                <= 55 => ItemRarity.Common,
                > 55 and <= 85 => ItemRarity.Rare,
                > 85 and <= 95 => ItemRarity.Epic,
                > 95 and <= 100 => ItemRarity.Legendary,
                _ => ItemRarity.Common
            };
        }
    }
}