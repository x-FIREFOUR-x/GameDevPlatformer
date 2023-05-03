using System;
using UnityEngine;

namespace Movement.Data
{
    [Serializable]
    public class JumpData
    {
        [field: SerializeField] public float GravityScale;
        [field: SerializeField] public Transform GroundCheck { get; private set; }
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    }
}
