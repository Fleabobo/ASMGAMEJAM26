using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Regen Settings")]
    public float regenDelay = 10f;
    public float regenRate = 5f;

    [Header("Sound")]
    public AudioSource audioSource;   // add an AudioSource component and drag it here
    public AudioClip damageSound;

    private float timeSinceLastHit;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        timeSinceLastHit += Time.deltaTime;

        if (timeSinceLastHit >= regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        timeSinceLastHit = 0f;

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
    }
}