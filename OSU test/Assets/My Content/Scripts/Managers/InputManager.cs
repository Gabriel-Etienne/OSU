using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Action<ButtonPressed> OnButtonPressedEvent;
    public static Action<ButtonPressed> OnButtonReleasedEvent;
    
    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnButtonPressedEvent.Invoke(context.ReadValue<ButtonPressed>());
            
        }
        else if (context.canceled)
        {
            OnButtonReleasedEvent.Invoke(context.ReadValue<ButtonPressed>());
        }
    }
}

public enum ButtonPressed
{
    Primary,
    Secondary
}
