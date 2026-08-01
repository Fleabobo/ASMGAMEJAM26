using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private string firstLevelSceneName = "Level1";

    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ShowLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }

    public void HideLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    public void ShowSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void HideSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}