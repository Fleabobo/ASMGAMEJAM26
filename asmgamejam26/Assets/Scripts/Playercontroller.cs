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

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        ConfigureFlashlight();
        ConfigureHorrorAtmosphere();
        gameObject.AddComponent<WallTorchSpawner>().SpawnTorches();

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
        flashlight.intensity = 100f;
        flashlight.range = 100f;
        flashlight.spotAngle = 60f;
        flashlight.innerSpotAngle = 35f;
        flashlight.shadows = LightShadows.Soft;
        flashlight.enabled = true;
        flashlight.transform.localPosition = new Vector3(0f, 0f, 0.2f);
    }

    private void ConfigureHorrorAtmosphere()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.18f, 0.20f, 0.24f, 1f);
        RenderSettings.ambientIntensity = 1.5f;
        RenderSettings.reflectionIntensity = 0f;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(0.18f, 0.20f, 0.24f, 1f);
        RenderSettings.fogStartDistance = 50f;
        RenderSettings.fogEndDistance = 250f;

        Light[] sceneLights = FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Light sceneLight in sceneLights)
        {
            if (sceneLight.type == LightType.Directional && sceneLight != flashlight)
            {
                sceneLight.enabled = false;
            }
        }

        if (flashlight != null)
        {
            flashlight.enabled = true;
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
