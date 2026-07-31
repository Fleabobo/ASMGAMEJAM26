using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class WallTorchSpawner : MonoBehaviour
{
    private const int MaxTorches = 12;
    private const float MinimumWallSize = 2f;
    private const float TorchRange = 14f;
    private const float TorchIntensity = 7f;
    private static readonly Color TorchColor = new(1f, 0.32f, 0.08f, 1f);

    /// <summary>
    /// Finds the level's far end relative to the player and places torches on nearby wall geometry.
    /// </summary>
    public void SpawnTorches()
    {
        if (GameObject.Find("Runtime Wall Torches") != null)
        {
            return;
        }

        List<MeshRenderer> wallRenderers = CollectWallRenderers(out Bounds levelBounds);
        if (wallRenderers.Count == 0)
        {
            return;
        }

        bool useX = levelBounds.size.x >= levelBounds.size.z;
        float playerAxis = useX ? transform.position.x : transform.position.z;
        float centerAxis = useX ? levelBounds.center.x : levelBounds.center.z;
        float farDirection = playerAxis <= centerAxis ? 1f : -1f;

        wallRenderers.Sort((left, right) =>
        {
            float leftAxis = useX ? left.bounds.center.x : left.bounds.center.z;
            float rightAxis = useX ? right.bounds.center.x : right.bounds.center.z;
            return (farDirection * rightAxis).CompareTo(farDirection * leftAxis);
        });

        GameObject torchRoot = new("Runtime Wall Torches");
        int created = 0;
        float lastPosition = float.NaN;

        foreach (MeshRenderer renderer in wallRenderers)
        {
            if (created >= MaxTorches)
            {
                break;
            }

            Bounds wallBounds = renderer.bounds;
            float wallAxis = useX ? wallBounds.center.x : wallBounds.center.z;
            if (!float.IsNaN(lastPosition) && Mathf.Abs(wallAxis - lastPosition) < 8f)
            {
                continue;
            }

            Vector3 torchPosition = wallBounds.center;
            torchPosition.y = Mathf.Clamp(wallBounds.max.y - 1.2f, wallBounds.min.y + 1.4f, wallBounds.max.y);
            if (useX)
            {
                torchPosition.x = farDirection > 0f ? wallBounds.max.x - 1f : wallBounds.min.x + 1f;
            }
            else
            {
                torchPosition.z = farDirection > 0f ? wallBounds.max.z - 1f : wallBounds.min.z + 1f;
            }

            CreateTorch(torchRoot.transform, torchPosition, created);
            lastPosition = wallAxis;
            created++;
        }

        if (created == 0)
        {
            Destroy(torchRoot);
        }
    }

    private static List<MeshRenderer> CollectWallRenderers(out Bounds levelBounds)
    {
        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsInactive.Exclude);
        List<MeshRenderer> wallRenderers = new();
        levelBounds = default;
        bool hasBounds = false;

        foreach (MeshRenderer renderer in renderers)
        {
            if (!IsWallCandidate(renderer.gameObject.name) || renderer.bounds.size.sqrMagnitude < MinimumWallSize * MinimumWallSize)
            {
                continue;
            }

            wallRenderers.Add(renderer);
            if (!hasBounds)
            {
                levelBounds = renderer.bounds;
                hasBounds = true;
            }
            else
            {
                levelBounds.Encapsulate(renderer.bounds);
            }
        }

        return wallRenderers;
    }

    private static bool IsWallCandidate(string objectName)
    {
        return objectName.Contains("Wall", StringComparison.OrdinalIgnoreCase)
            || objectName.Contains("Shell", StringComparison.OrdinalIgnoreCase)
            || objectName.Contains("Closure", StringComparison.OrdinalIgnoreCase);
    }

    private static void CreateTorch(Transform parent, Vector3 position, int index)
    {
        GameObject torch = new($"Wall Torch {index + 1}");
        torch.transform.SetParent(parent, true);
        torch.transform.position = position;

        Light light = torch.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = TorchColor;
        light.intensity = TorchIntensity;
        light.range = TorchRange;
        light.shadows = LightShadows.None;

        GameObject flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flame.name = "Flame";
        flame.transform.SetParent(torch.transform, false);
        flame.transform.localPosition = new Vector3(0f, 0.35f, 0f);
        flame.transform.localScale = new Vector3(0.28f, 0.5f, 0.28f);

        Collider flameCollider = flame.GetComponent<Collider>();
        if (flameCollider != null)
        {
            Destroy(flameCollider);
        }

        Renderer flameRenderer = flame.GetComponent<Renderer>();
        if (flameRenderer != null)
        {
            Material material = new(Shader.Find("Universal Render Pipeline/Lit"));
            material.color = new Color(1f, 0.18f, 0.02f, 1f);
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", new Color(2f, 0.12f, 0.01f, 1f));
            flameRenderer.material = material;
        }
    }
}
