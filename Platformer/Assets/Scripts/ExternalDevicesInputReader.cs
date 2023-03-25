using UnityEngine;
using UnityEngine.EventSystems;

public class ExternalDevicesInputReader: IEentityInputSource
{
    public float HorizontalDirection => Input.GetAxisRaw("Horizontal");

    public bool Attack { get; private set; }
    public bool Jump { get; private set; }
    public bool Roll { get; private set; }

    public bool Block => Input.GetButton("Fire2");

    public void OnUpdate()
    {
        if (!IsPointerOverUi() && Input.GetButtonDown("Fire1"))
            Attack = true;

        if (Input.GetButtonDown("Jump"))
            Jump = true;

        if (Input.GetButtonDown("Debug Multiplier"))
            Roll = true;
    }

    private bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject();

    public void ResetOneTimeActions()
    {
        Attack = false;
        Jump = false;
        Roll = false;
    }
}
