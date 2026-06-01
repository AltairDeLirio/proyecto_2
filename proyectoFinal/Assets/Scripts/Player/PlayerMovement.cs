using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float groundStickForce = -2f;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;

    private DialogueRunner runner;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        runner = FindFirstObjectByType<DialogueRunner>();
    }

    void Update()
    {
        // Block movement during dialogue
        if (runner != null && runner.IsDialogueRunning)
            return;

        // Grounded check
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = groundStickForce;
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Movement input
        Vector3 move =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        // Combine horizontal + vertical movement
        Vector3 velocity = move * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}