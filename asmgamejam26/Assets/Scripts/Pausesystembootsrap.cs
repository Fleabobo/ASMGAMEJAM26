using UnityEngine;

/// <summary>
/// Automatically spawns the PauseSystem prefab once, before any scene loads.
/// Requires the prefab at: Assets/Resources/PauseSystem.prefab
/// (must be inside a folder named exactly "Resources", and named exactly "PauseSystem").
/// </summary>
public static class PauseSystemBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (PauseMenu.Instance != null) return;

        GameObject prefab = Resources.Load<GameObject>("PauseSystem");
        if (prefab == null)
        {
            Debug.LogError("PauseSystemBootstrap: Could not find 'PauseSystem' prefab in a Resources folder.");
            return;
        }

        Object.Instantiate(prefab);
    }
}