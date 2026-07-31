using UnityEngine;
using UnityEngine.Rendering;

public sealed class Level1DarkEnvironment : MonoBehaviour
{
    private static readonly Color AmbientColor = new(0.0705882f, 0.0705882f, 0.0784314f, 1f);
    private static readonly Color FogColor = new(0.0313726f, 0.0313726f, 0.0392157f, 1f);

    [SerializeField] private Camera targetCamera;

    private void Awake()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = AmbientColor;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 0f;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogDensity = 0f;
        RenderSettings.fogStartDistance = 6f;
        RenderSettings.fogEndDistance = 35f;

        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.backgroundColor = Color.black;
        }
    }
}
