using System;
using System.Collections.Generic;
using UnityEngine;

using Items.Behaviour;
using Items.Core;
using Items.Data;
using Items.Rarity;

namespace Items
{
    public class ItemsSystem : IDisposable
    {
        private readonly SceneItem _sceneItem;
        private readonly Transform _transform;
        private readonly List<IItemRarityColor> _colors;
        private readonly LayerMask _whatIsPlayer;
        private readonly ItemsFactory _itemsFactory;
        private readonly Inventory _inventory;

        private readonly Dictionary<SceneItem, Item> _itemsOnScene;

        public ItemsSystem(List<IItemRarityColor> colors, LayerMask whatIsPlayer, ItemsFactory itemsFactory, Inventory inventory)
        {
            _sceneItem = Resources.Load<SceneItem>($"{"Prefabs"}/{"Items"}/{nameof(SceneItem)}");
            _itemsOnScene = new Dictionary<SceneItem, Item>();
            GameObject gameObject = new GameObject();
            gameObject.name = nameof(ItemsSystem);
            _transform = gameObject.transform;
            _colors = colors;
            _whatIsPlayer = whatIsPlayer;
            _itemsFactory = itemsFactory;
            _inventory = inventory;
            _inventory.ItemDropped += DropItem;
        }

        public void Dispose()
        {
            _inventory.ItemDropped -= DropItem;
        }

        public void DropItem(ItemDescriptor descriptor, Vector2 position)
        {
            Item item = _itemsFactory.CreateItem(descriptor);
            DropItem(item, position);
        }

        private void DropItem(Item item, Vector2 position)
        {
            SceneItem sceneItem = UnityEngine.Object.Instantiate(_sceneItem, _transform);
            sceneItem.SetItem(item.Descriptor.ItemSprite, item.Descriptor.ItemId.ToString(),
                _colors.Find(color => color.ItemRarity == item.Descriptor.ItemRarity).Color);
            sceneItem.PlayAnimationDrop(position);
            sceneItem.ItemClicked += TryPickItem;
            _itemsOnScene.Add(sceneItem, item);
        }

        private void TryPickItem(SceneItem sceneItem)
        {
            Collider2D player = Physics2D.OverlapCircle(sceneItem.Position, sceneItem.InteractionDistance, _whatIsPlayer);
            if (player == null)
                return;

            Item item = _itemsOnScene[sceneItem];

            if (!_inventory.TryAddItemToBackPack(item))
                return;
            _itemsOnScene.Remove(sceneItem);
            sceneItem.ItemClicked -= TryPickItem;
            UnityEngine.Object.Destroy(sceneItem.gameObject);
        }
    }
}