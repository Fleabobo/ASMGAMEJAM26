using System.Collections;
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

    [Header("Combat & Slash Settings")]
    public float attackRange = 2.5f;
    public float attackCooldown = 0.35f;
    public GameObject slashUIOverlay;
    public LayerMask enemyLayer;

    [Header("Flashlight Settings")]
    public Light flashlight;

    // Internal variables
    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0.0f;
    private bool canAttack = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        ConfigureFlashlight();
        ConfigureHorrorAtmosphere();

        // Lock cursor to screen center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (slashUIOverlay != null)
            slashUIOverlay.SetActive(false);
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
        HandleAttack();
    }

    void HandleLook()
    {
        if (cameraTransform == null || Mouse.current == null) return;

        // New Input System mouse delta
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        // Rotate body horizontally
        transform.Rotate(Vector3.up * mouseDelta.x);

        // Clamp vertical camera look
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

        // Read WASD keys from New Input System
        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.wKey.isPressed) z += 1f;

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleFlashlight()
    {
        // Toggle flashlight on 'F' key press
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    void HandleAttack()
    {
        // Left Mouse Click check in New Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canAttack)
        {
            StartCoroutine(PerformSlash());
        }
    }

    IEnumerator PerformSlash()
    {
        canAttack = false;

        if (slashUIOverlay != null)
            slashUIOverlay.SetActive(true);

        DetectHit();

        yield return new WaitForSeconds(0.12f);

        if (slashUIOverlay != null)
            slashUIOverlay.SetActive(false);

        yield return new WaitForSeconds(attackCooldown - 0.12f);
        canAttack = true;
    }

    void DetectHit()
    {
        RaycastHit hit;
        Vector3 rayOrigin = cameraTransform.position;
        Vector3 rayDirection = cameraTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, attackRange, enemyLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * attackRange);
        }
    }
}