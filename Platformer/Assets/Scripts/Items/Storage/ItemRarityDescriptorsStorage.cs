using System;
using System.Collections.Generic;
using Items.Data;
using UnityEngine;

namespace Items.Storage
{
    [CreateAssetMenu(fileName = "ItemsSystem", menuName = "ItemSystem/ItemRarirtyDescriptorsStorage")]
    public class ItemRarityDescriptorsStorage: ScriptableObject
    {
        [field: SerializeField] public List<RarityDescriptor> RarityDescriptors { get; private set; }
    }
}