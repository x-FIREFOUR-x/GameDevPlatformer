using System.Collections.Generic;
using Items.Scriptable;
using UnityEngine;

namespace Items.Storage
{
    [CreateAssetMenu(fileName = "ItemsStorage", menuName = "ItemsSystem/ItemStorage")]
    public class ItemStorage : ScriptableObject
    {
        [field: SerializeField] public List<BaseItemScriptable> ItemScriptables { get; private set; }
    }
}