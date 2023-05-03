using UnityEngine;

using Items.Data;

namespace Items.Scriptable
{
    [CreateAssetMenu(fileName = "Item", menuName = "ItemsSystem/Item")]
    public class ItemScriptable : BaseItemScriptable
    {
        [SerializeField] private StatChangingItemDescriptor _itemDescriptor;
        public override ItemDescriptor ItemDescriptor => _itemDescriptor;

        public ItemScriptable(StatChangingItemDescriptor descriptor)
        {
            _itemDescriptor = descriptor;
        }
    }
}