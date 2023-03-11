using UnityEngine;
using System;
using System.Collections.Generic;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private List<ParallaxLayer> _layers;
    [SerializeField] private Transform _target;

    private float _previousTargetPosition;

    private void Start()
    {
        _previousTargetPosition = _target.position.x;
    }

    private void LateUpdate()
    {
        float deltaMovement = _previousTargetPosition - _target.position.x;
        foreach (var layer in _layers)
        {
            Vector2 layerPosition = layer.Transform.position;
            layerPosition.x = layerPosition.x + deltaMovement * layer.Speed;
            layer.Transform.position = layerPosition;
        }

        _previousTargetPosition = _target.position.x;
    }

    [Serializable]
    private class ParallaxLayer
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}
