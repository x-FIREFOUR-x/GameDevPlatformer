using System.Collections.Generic;
using UnityEngine;

using Player;

class GameLevelInitializer: MonoBehaviour
{
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private GameUIInputView _ganeUIInputView;

    private ExternalDevicesInputReader _externalDevicesInput;
    private PlayerBrain _playerBrain;

    private bool _onPause = true;

    private void Awake()
    {
        _externalDevicesInput = new ExternalDevicesInputReader();

        _playerBrain = new PlayerBrain(_playerEntity, new List<IEentityInputSource>
        {
            _ganeUIInputView,
            _externalDevicesInput
        });
    }

    private void Update()
    {
        if (_onPause)
            return;

        _externalDevicesInput.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (_onPause)
            return;

        _playerBrain.OnFixedUpdate();
    }

    public void PauseOff()
    {
        _onPause = false;
    }
}
