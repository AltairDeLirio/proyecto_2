using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float sensitivity = 0.15f;

    [Header("Vertical Limits")]
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch;

    private Vector2 lookInput;

    private DialogueRunner runner;

    void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();

        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        if (runner != null && runner.IsDialogueRunning)
            return;

        RotateCamera();
    }

    void RotateCamera()
    {
        yaw += lookInput.x * sensitivity;

        pitch -= lookInput.y * sensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch);

        transform.rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}