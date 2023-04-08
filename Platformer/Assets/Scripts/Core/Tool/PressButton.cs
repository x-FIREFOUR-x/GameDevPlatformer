using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent buttonPressed;
    public UnityEvent buttonUnpressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonUnpressed.Invoke();
    }
}
