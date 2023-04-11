using System.Collections.Generic;
using System.Linq;
using Core.Services.Updater;
using Items.Data;
using Items.Enum;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class DropGenerator
    {
        private PlayerEntity _playerEntity;
        private List<ItemDescriptor> _itemDescriptors;
        private ItemsSystem _itemsSystem;

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
            ItemDescriptor itemDescriptor = items[Random.Range(0, items.Count)];
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
                <= 65 => ItemRarity.Common,
                > 65 and <= 90 => ItemRarity.Rare,
                > 90 and <= 100 => ItemRarity.Epic,
                _ => ItemRarity.Common
            };
        }
    }
}