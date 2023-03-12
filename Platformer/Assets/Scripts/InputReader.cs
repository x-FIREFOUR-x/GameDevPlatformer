using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private Player.PlayerEntity _playerEntity;

    private void Update()
    {
        _playerEntity.Block(Input.GetButton("Fire2"));

        _playerEntity.Move(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButtonDown("Debug Multiplier"))
            _playerEntity.Roll();

        if (Input.GetButtonDown("Jump"))
            _playerEntity.Jump();

        if (Input.GetButtonDown("Fire1"))
            _playerEntity.Attack();
    }
}
