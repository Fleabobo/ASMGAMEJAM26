using UnityEngine;

public static class LevelProgress
{
    private const string UnlockedLevelKey = "UnlockedLevel";

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(UnlockedLevelKey, 1);
    }

    public static void UnlockLevel(int levelNumber)
    {
        int current = GetUnlockedLevel();
        if (levelNumber > current)
        {
            PlayerPrefs.SetInt(UnlockedLevelKey, levelNumber);
            PlayerPrefs.Save();
        }
    }

    public static bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber <= GetUnlockedLevel();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.SetInt(UnlockedLevelKey, 1);
        PlayerPrefs.Save();
    }
}