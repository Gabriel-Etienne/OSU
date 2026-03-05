using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTrailFollow : MonoBehaviour
{
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }
    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        transform.position = mouseWorldPos;
    }
}
