using UnityEngine;
using UnityEngine.Rendering;

public sealed class Level1DarkEnvironment : MonoBehaviour
{
    private static readonly Color AmbientColor = new(0.18f, 0.20f, 0.24f, 1f);
    private static readonly Color FogColor = new(0.18f, 0.20f, 0.24f, 1f);

    [SerializeField] private Camera targetCamera;

    private void Awake()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = AmbientColor;
        RenderSettings.ambientIntensity = 1.5f;
        RenderSettings.reflectionIntensity = 0f;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogDensity = 0f;
        RenderSettings.fogStartDistance = 50f;
        RenderSettings.fogEndDistance = 250f;

        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = Color.black;
        }
    }
}
