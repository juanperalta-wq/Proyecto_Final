using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [FoldoutGroup("Variables")]
    [SerializeField]public InputSystem_Actions inputs;
    [FoldoutGroup("Variables")]
    [SerializeField]private CharacterController controller;
    [FoldoutGroup("Variables")]
    [SerializeField] private Transform cameraHolder;

    [HorizontalGroup("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;

    [HorizontalGroup("Jump")]
    public float jumpForce = 8f;
    public float gravity = -20f;

    [HorizontalGroup("Mouse")]
    public float mouseSensitivity = 0.20f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float verticalVelocity;
    private float xRotation;

    private bool isSprinting;

    private void Awake()
    {
        inputs = new();

        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputs.Enable();

        // MOVE
        inputs.Player.Move.performed += ctx =>
            moveInput = ctx.ReadValue<Vector2>();

        inputs.Player.Move.canceled += ctx =>
            moveInput = Vector2.zero;

        // LOOK
        inputs.Player.Look.performed += ctx =>
            lookInput = ctx.ReadValue<Vector2>();

        inputs.Player.Look.canceled += ctx =>
            lookInput = Vector2.zero;

        // JUMP
        inputs.Player.Jump.performed += OnJump;

        // SPRINT
        inputs.Player.Sprint.performed += ctx =>
            isSprinting = true;

        inputs.Player.Sprint.canceled += ctx =>
            isSprinting = false;
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
    {
        OnLook();
        OnMove();
    }

    public void OnMove()
    {
        float currentSpeed =
            isSprinting ? sprintSpeed : moveSpeed;

        Vector3 moveDir =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        moveDir *= currentSpeed;

        // GRAVITY
        verticalVelocity += gravity * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        moveDir.y = verticalVelocity;

        controller.Move(moveDir * Time.deltaTime);
    }

    public void OnLook()
    {
        float mouseX =
            lookInput.x * mouseSensitivity;

        float mouseY =
            lookInput.y * mouseSensitivity;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded) return;

        verticalVelocity = jumpForce;
    }
}