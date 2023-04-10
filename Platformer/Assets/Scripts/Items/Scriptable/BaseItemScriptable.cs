using Items.Data;
using UnityEngine;

namespace Items.Scriptable
{
    public abstract class BaseItemScriptable : ScriptableObject
    {
        public abstract ItemDescriptor ItemDescriptor { get;  }
    }
}