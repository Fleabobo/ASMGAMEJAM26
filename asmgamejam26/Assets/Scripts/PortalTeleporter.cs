using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string spawnPointId = "PortalSpawn";
    [SerializeField] private int currentLevelNumber = 1; // which level this portal is exiting FROM

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelProgress.UnlockLevel(currentLevelNumber + 1);

            TeleportManager.NextSpawnId = spawnPointId;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}