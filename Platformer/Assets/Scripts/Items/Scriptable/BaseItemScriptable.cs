using UnityEngine;

using Items.Data;

namespace Items.Scriptable
{
    public abstract class BaseItemScriptable : ScriptableObject
    {
        public abstract ItemDescriptor ItemDescriptor { get;  }
    }
}