using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private string spawnId = "PortalSpawn";

    private void Start()
    {
        if (TeleportManager.NextSpawnId == spawnId)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = transform.position;
                player.transform.rotation = transform.rotation;
            }
            TeleportManager.NextSpawnId = null;
        }
    }
}