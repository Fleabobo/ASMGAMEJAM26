using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsText;
    public float scrollSpeed = 50f;
    public string mainMenuSceneName = "MainMenu";
    public bool returnToMenuWhenDone = true;

    private float endYPosition;

    void Start()
    {
        // Estimate how far it needs to scroll based on text height
        endYPosition = creditsText.rect.height + 1200f; // adjust buffer as needed
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (returnToMenuWhenDone && creditsText.anchoredPosition.y >= endYPosition)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}