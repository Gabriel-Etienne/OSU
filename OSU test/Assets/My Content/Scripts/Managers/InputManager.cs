using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Action<ButtonPressed> OnButtonPressedEvent;
    public static Action<ButtonPressed> OnButtonReleasedEvent;
    
    public static Action OnFastForwardEvent;
    public static Action OnFastBackwardEvent;
    public static Action OnPlayEvent;
    public static Action OnRestartEvent;
    
    public void OnFastForward(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFastForwardEvent?.Invoke();
        }
    }
    
    public void OnFastBackward(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFastBackwardEvent?.Invoke();
        }
    }
    
    public void OnPlay(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPlayEvent?.Invoke();
        }
    }
    
    
    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRestartEvent?.Invoke();
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
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
            OnButtonPressedEvent?.Invoke(value);
            
        }
        else if (context.canceled)
        {
            OnButtonReleasedEvent?.Invoke(value);
        }
        
    }
    
}

public enum ButtonPressed
{
    Primary,
    Secondary
}
