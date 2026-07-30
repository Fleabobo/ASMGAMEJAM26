using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Regen Settings")]
    public float regenDelay = 10f;      // seconds after last hit before regen starts
    public float regenRate = 5f;        // health per second while regenerating

    private float timeSinceLastHit;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        timeSinceLastHit += Time.deltaTime;

        // Only regen if enough time has passed since last hit, and not already full
        if (timeSinceLastHit >= regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // don't overshoot
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f); // don't go below 0
        timeSinceLastHit = 0f; // reset regen timer every time we get hit

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        // add death logic here later (restart level, show game over screen, etc.)
    }
}