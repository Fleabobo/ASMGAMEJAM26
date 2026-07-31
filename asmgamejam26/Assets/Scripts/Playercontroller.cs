using UnityEngine;
using UnityEngine.InputSystem; // Uses the New Input System package

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

    // Internal variables
    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        ConfigureFlashlight();
        ConfigureHorrorAtmosphere();

        // Lock cursor to screen center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ConfigureFlashlight()
    {
        if (flashlight == null)
        {
            return;
        }

        flashlight.type = LightType.Spot;
        flashlight.intensity = 35f;
        flashlight.range = 25f;
        flashlight.spotAngle = 55f;
        flashlight.enabled = true;
        flashlight.transform.localPosition = new Vector3(0f, 0f, 0.2f);
    }

    private void ConfigureHorrorAtmosphere()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.0705882f, 0.0705882f, 0.0784314f, 1f);
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 0f;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(0.0313726f, 0.0313726f, 0.0392157f, 1f);
        RenderSettings.fogStartDistance = 6f;
        RenderSettings.fogEndDistance = 35f;

        Light[] sceneLights = FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Light sceneLight in sceneLights)
        {
            if (sceneLight.type == LightType.Directional)
            {
                sceneLight.enabled = false;
            }
        }
    }

    void Update()
    {
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
    }

    void HandleFlashlight()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}