using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private PlayerEntity _playerEntity;

    private float _directionMove;

    private void Update()
    {
        _directionMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            _playerEntity.Jump();
    }

    private void FixedUpdate()
    {
        _playerEntity.Move(_directionMove);
    }
}
