using System;
using UnityEngine;

namespace Movement.Data
{
    [Serializable]
    public class RollData
    {
        [field:SerializeField] public float RollSpeed;
        [field: SerializeField] public float RollDistance;

        [field: SerializeField] public Vector2 OffsetRollCollider;
        [field: SerializeField] public Vector2 SizeRollCollider;
    }
}
