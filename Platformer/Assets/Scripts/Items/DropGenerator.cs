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
        private readonly PlayerEntity _playerEntity;
        private readonly List<ItemDescriptor> _itemDescriptors;
        private readonly ItemsSystem _itemsSystem;

        public DropGenerator(List<ItemDescriptor> itemDescriptors, PlayerEntity playerEntity, ItemsSystem itemsSystem)
        {
            _playerEntity = playerEntity;
            _itemDescriptors = itemDescriptors;
            _itemsSystem = itemsSystem;
            ProjectUpdater.Instance.UpdateCalled += Update;
        }

        private void DropRandomItem(ItemRarity rarity)
        {
            List<ItemDescriptor> items = _itemDescriptors.Where(item => item.ItemRarity == rarity).ToList();
            ItemDescriptor itemDescriptor = items[UnityEngine.Random.Range(0, items.Count)];
            _itemsSystem.DropItem(itemDescriptor, _playerEntity.transform.position + UnityEngine.Vector3.one);
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.G))
                DropRandomItem(GetDropRarity());
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