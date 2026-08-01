using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class LevelSelectPanel : MonoBehaviour
{
    [System.Serializable]
    public sealed class LevelEntry
    {
        public int levelNumber;
        public Button button;
        public string sceneName;
        public GameObject lockedOverlay;
    }

    [SerializeField] private LevelEntry[] levels;

    private void OnEnable()
    {
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        foreach (LevelEntry entry in levels)
        {
            if (entry.button == null) continue;

            bool unlocked = LevelProgress.IsLevelUnlocked(entry.levelNumber);
            entry.button.interactable = unlocked;

            if (entry.lockedOverlay != null)
            {
                entry.lockedOverlay.SetActive(!unlocked);
            }

            string sceneToLoad = entry.sceneName;
            entry.button.onClick.RemoveAllListeners();
            entry.button.onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
        }
    }
}