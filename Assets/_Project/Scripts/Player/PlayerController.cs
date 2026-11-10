using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player movement attributes")]
    [SerializeField] float walkSpeed = 2.5f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Transform cameraTransform;
    Animator playerAnimator;
    [SerializeField] float speedSmoothTime = 0.15f;
    CharacterController playerController;
    [SerializeField] float gravity = -20f;
    float verticalVelocity;


    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerController = GetComponent<CharacterController>();
    }
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool isRunning = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        if (Gamepad.current != null && Gamepad.current.buttonEast.isPressed)
            isRunning = true;

        float moveSpeed = walkSpeed;
        if (isRunning)
            moveSpeed = runSpeed;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        Vector3 direction = camForward.normalized * z + camRight.normalized * x;

        playerAnimator.SetFloat("Speed", direction.normalized.magnitude * moveSpeed, speedSmoothTime, Time.deltaTime);

        if (playerController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = direction.normalized * moveSpeed;
        move.y = verticalVelocity;

        playerController.Move(move * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion target = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }
    }
}
