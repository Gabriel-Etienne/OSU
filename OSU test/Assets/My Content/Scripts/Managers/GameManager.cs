using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.OnButtonPressedEvent += StartCheck;
        InputManager.OnButtonReleasedEvent += EndCheck;
    }

    private void OnDisable()
    {
        InputManager.OnButtonPressedEvent -= StartCheck;
        InputManager.OnButtonReleasedEvent -= EndCheck;
        
    }

    private void StartCheck(ButtonPressed buttonPressed)
    {
        // start coroutine of check 
        Debug.Log("Button pressed");
    }

    private void EndCheck(ButtonPressed buttonPressed)
    {
        // end coroutine of check 
        Debug.Log("Button released");
    }
    
    
}
