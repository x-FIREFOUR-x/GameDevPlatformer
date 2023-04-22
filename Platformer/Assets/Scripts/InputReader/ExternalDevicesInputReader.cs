using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Core.Services.Updater;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource, IWindowsInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public bool Attack { get; private set; }
        public bool Jump { get; private set; }
        public bool Roll { get; private set; }
        public bool Block => Input.GetButton("Fire2");

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public event Action InventoryRequested;

        public void ResetOneTimeActions()
        {
            Attack = false;
            Jump = false;
            Roll = false;
        }

        private void OnUpdate()
        {
            if (!IsPointerOverUi() && Input.GetButtonDown("Fire1"))
                Attack = true;

            if (Input.GetButtonDown("Jump"))
                Jump = true;

            if (Input.GetButtonDown("Debug Multiplier"))
                Roll = true;

            if (Input.GetKeyDown(KeyCode.E))
                InventoryRequested?.Invoke();
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject();
    }
}
