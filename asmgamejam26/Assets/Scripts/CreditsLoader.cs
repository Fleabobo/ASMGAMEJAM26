using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsLoader : MonoBehaviour
{
    public static CreditsLoader Instance;

    void Awake()
    {
        Instance = this;
    }

    public void LoadCreditsAfterDelay(string sceneName, float delay)
    {
        StartCoroutine(Routine(sceneName, delay));
    }

    IEnumerator Routine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}