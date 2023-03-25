using System;
using UnityEngine;

namespace Movement.Data
{
    [Serializable]
    public class MovementData
    {
        [field: SerializeField] public float MoveSpeed;
        [field: SerializeField] public bool FaceRight;
    }
}
