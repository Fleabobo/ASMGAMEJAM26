using UnityEngine;
using UnityEngine.Rendering;

public sealed class Level1DarkEnvironment : MonoBehaviour
{
    private static readonly Color AmbientColor = new(0.008f, 0.008f, 0.008f, 1f);
    private static readonly Color FogColor = new(0.004f, 0.006f, 0.01f, 1f);

    [SerializeField] private Camera targetCamera;

    private void Awake()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = AmbientColor;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 0f;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogDensity = 0.05f;
        RenderSettings.fogStartDistance = 0f;
        RenderSettings.fogEndDistance = 15f;

        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = Color.black;
        }
    }
}
