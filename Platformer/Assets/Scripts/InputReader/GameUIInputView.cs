using System;
using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    class GameUIInputView : MonoBehaviour, IEntityInputSource, IWindowsInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _rollButton;
        [SerializeField] private PressButton _blockButton;

        [SerializeField] private Button _inventoryButton;

        public event Action InventoryRequested;

        public float HorizontalDirection => _joystick.Horizontal;
        public bool Attack { get; private set; }
        public bool Jump { get; private set; }
        public bool Roll { get; private set; }
        public bool Block { get; private set; }

        private void Awake()
        {
            _attackButton.onClick.AddListener(() => Attack = true);
            _jumpButton.onClick.AddListener(() => Jump = true);
            _rollButton.onClick.AddListener(() => Roll = true);

            _blockButton.buttonPressed.AddListener(() => Block = true);
            _blockButton.buttonUnpressed.AddListener(() => Block = false);

            _inventoryButton.onClick.AddListener(() => InventoryRequested?.Invoke());
        }

        public void ResetOneTimeActions()
        {
            Attack = false;
            Jump = false;
            Roll = false;
        }

        private void OnDestroy()
        {
            _attackButton.onClick.RemoveAllListeners();
            _jumpButton.onClick.RemoveAllListeners();
            _rollButton.onClick.RemoveAllListeners();

            _blockButton.buttonPressed.RemoveAllListeners();
            _blockButton.buttonUnpressed.RemoveAllListeners();

            _inventoryButton.onClick.RemoveAllListeners();
        }
    }
}
