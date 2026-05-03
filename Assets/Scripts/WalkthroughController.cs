using UnityEngine;
using UnityEngine.InputSystem;

public class WalkthroughController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float lookSensitivity = 0.15f;
    public float eyeHeight = 1.25f;

    private CharacterController controller;
    private Transform cameraTransform;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null) controller = gameObject.AddComponent<CharacterController>();
        
        // Setup Camera
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            cameraTransform = mainCam.transform;
            cameraTransform.SetParent(this.transform);
            cameraTransform.localPosition = new Vector3(0, eyeHeight, 0);
            cameraTransform.localRotation = Quaternion.identity;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // 1. Rotation (Mouse)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * lookSensitivity;
        float mouseY = mouseDelta.y * lookSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 2. Movement (Keyboard)
        float moveX = 0;
        float moveZ = 0;

        if (Keyboard.current.wKey.isPressed) moveZ += 1;
        if (Keyboard.current.sKey.isPressed) moveZ -= 1;
        if (Keyboard.current.aKey.isPressed) moveX -= 1;
        if (Keyboard.current.dKey.isPressed) moveX += 1;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);
        
        // Apply simple gravity to stay on floor
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }

        // Unlock cursor with Escape
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
