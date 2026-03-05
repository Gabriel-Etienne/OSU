using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Action<ButtonPressed> OnButtonPressedEvent;
    public static Action<ButtonPressed> OnButtonReleasedEvent;
    
    public void OnButtonPrimaryPressed(InputAction.CallbackContext context)
    {
        OnButtonPressed(context, ButtonPressed.Primary);
    }
    public void OnButtonSecondaryPressed(InputAction.CallbackContext context)
    {
        OnButtonPressed(context, ButtonPressed.Secondary);
    }

    private void OnButtonPressed(InputAction.CallbackContext context, ButtonPressed value)
    {
        if (context.performed)
        {
            OnButtonPressedEvent.Invoke(value);
            
        }
        else if (context.canceled)
        {
            OnButtonReleasedEvent.Invoke(value);
        }
        
    }
    
}

public enum ButtonPressed
{
    Primary,
    Secondary
}
