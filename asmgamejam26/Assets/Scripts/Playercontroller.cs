using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Playercontroller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float gravity = -19.62f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 0.1f;
    public float maxLookAngle = 80.0f;

    [Header("Flashlight Settings")]
    public Light flashlight;

    [Header("Footstep Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f; // seconds between steps while walking

    // Internal variables
    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0.0f;
    private float stepTimer;

    // Set true (e.g. by PlayerHealth) to freeze movement/look, such as while dead.
    public bool inputLocked = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (inputLocked) return;

        HandleLook();
        HandleMovement();
        HandleFlashlight();
    }

    void HandleLook()
    {
        if (cameraTransform == null || Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseDelta.x);

        cameraPitch -= mouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Keyboard.current == null) return;

        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.wKey.isPressed) z += 1f;

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Footsteps: only play while actually moving and grounded
        bool isMoving = move.magnitude > 0.1f && isGrounded;
        HandleFootsteps(isMoving);
    }

    void HandleFootsteps(bool isMoving)
    {
        if (!isMoving)
        {
            stepTimer = 0f; // reset so a step plays immediately next time you start moving
            return;
        }

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepAudioSource == null || footstepSounds == null || footstepSounds.Length == 0) return;

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        footstepAudioSource.PlayOneShot(clip);
    }

    void HandleFlashlight()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    /// <summary>
    /// Safely moves the player to a new position/rotation.
    /// CharacterController must be disabled while repositioning directly,
    /// otherwise Unity's collision resolution can block or fight the move.
    /// Also clears any accumulated fall/gravity velocity.
    /// </summary>
    public void Teleport(Vector3 position, Quaternion rotation)
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        controller.enabled = false;

        transform.position = position;
        transform.rotation = rotation;

        velocity = Vector3.zero;
        cameraPitch = 0f;
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.identity;
        }

        controller.enabled = true;
    }
}