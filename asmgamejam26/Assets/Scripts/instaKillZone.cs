using UnityEngine;

public class InstaKillZone : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null)
        {
            health = other.GetComponentInParent<PlayerHealth>();
        }

        if (health != null)
        {
            health.TakeDamage(health.maxHearts);
        }
    }
}