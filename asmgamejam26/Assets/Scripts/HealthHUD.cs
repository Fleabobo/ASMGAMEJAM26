using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [Header("Display")]
    [Min(1)]
    public int heartFontSize = 64;
    public Color fullHeartColor = new Color(0.95f, 0.05f, 0.08f, 1f);
    public Color emptyHeartColor = new Color(0.25f, 0.25f, 0.25f, 1f);
    [Min(0)]
    public int leftMargin = 28;
    [Min(0)]
    public int topMargin = 20;
    [Min(0)]
    public int heartSpacing = 6;

    private readonly List<Text> heartLabels = new();
    private PlayerHealth playerHealth;
    private Canvas canvas;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("HealthHUD requires a PlayerHealth component on the same GameObject.", this);
            enabled = false;
            return;
        }

        CreateCanvas();
        CreateHeartLabels();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged += Refresh;
            Refresh(playerHealth.CurrentHearts, playerHealth.maxHearts);
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged -= Refresh;
        }
    }

    private void CreateCanvas()
    {
        GameObject canvasObject = new GameObject("HealthHUDCanvas");
        canvasObject.transform.SetParent(transform, false);

        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();
    }

    private void CreateHeartLabels()
    {
        for (int index = 0; index < 5; index++)
        {
            GameObject heartObject = new GameObject($"Heart{index + 1}");
            heartObject.transform.SetParent(canvas.transform, false);

            RectTransform rectTransform = heartObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.anchoredPosition = new Vector2(leftMargin + index * (heartFontSize + heartSpacing), -topMargin);
            rectTransform.sizeDelta = new Vector2(heartFontSize + heartSpacing, heartFontSize + 12);

            Text label = heartObject.AddComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.text = "♥";
            label.fontSize = heartFontSize;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            label.raycastTarget = false;
            label.color = emptyHeartColor;
            heartLabels.Add(label);
        }
    }

    private void Refresh(int currentHearts, int maximumHearts)
    {
        int visibleHearts = Mathf.Min(maximumHearts, heartLabels.Count);
        for (int index = 0; index < heartLabels.Count; index++)
        {
            heartLabels[index].text = "♥";
            heartLabels[index].color = index < currentHearts && index < visibleHearts ? fullHeartColor : emptyHeartColor;
        }
    }
}
