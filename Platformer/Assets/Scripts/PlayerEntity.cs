using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerEntity : MonoBehaviour
{
    

    [Header("Move")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _faceRight;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityScale;

    private Rigidbody2D _rigidbody;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    public void Move(float direction)
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = direction * _moveSpeed;
        _rigidbody.velocity = velocity;

        SetDirection(direction);
    }

    public void Jump()
    {
        if (_rigidbody.velocity.y != 0)
            return;

        _rigidbody.AddForce(Vector2.up * _jumpForce);
        _rigidbody.gravityScale = _gravityScale;
    }


    private void SetDirection(float direction)
    {
        if ((_faceRight && direction < 0) || (!_faceRight && direction > 0))
            Flip();
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        _faceRight = !_faceRight;
    }
}
