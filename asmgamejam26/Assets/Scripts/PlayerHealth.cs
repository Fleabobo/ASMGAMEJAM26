using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Min(1)]
    public int maxHearts = 5;
    [SerializeField, Min(0)]
    private int currentHearts;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip damageSound;

    [Header("Respawn")]
    [Tooltip("Delay in seconds before the level reloads after death.")]
    public float respawnDelay = 1f;

    public int CurrentHearts => currentHearts;
    public bool IsDead => isDead;
    public event Action<int, int> HealthChanged;
    public event Action Died;

    private bool isDead;
    private Playercontroller playercontroller;

    private void Awake()
    {
        maxHearts = Mathf.Max(1, maxHearts);
        currentHearts = maxHearts;
        playercontroller = GetComponent<Playercontroller>();
    }

    private void Start()
    {
        HealthChanged?.Invoke(currentHearts, maxHearts);
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f)
        {
            return;
        }

        int heartsToLose = Mathf.Max(1, Mathf.CeilToInt(amount));
        SetHearts(currentHearts - heartsToLose);

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHearts <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount = 1)
    {
        if (isDead || amount <= 0)
        {
            return;
        }

        SetHearts(currentHearts + amount);
    }

    public void RestoreFullHealth()
    {
        if (isDead)
        {
            return;
        }

        SetHearts(maxHearts);
    }

    private void SetHearts(int value)
    {
        int clampedValue = Mathf.Clamp(value, 0, maxHearts);
        if (clampedValue == currentHearts)
        {
            return;
        }

        currentHearts = clampedValue;
        HealthChanged?.Invoke(currentHearts, maxHearts);
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (playercontroller != null)
        {
            playercontroller.inputLocked = true;
        }

        Debug.Log("Player died", this);
        Died?.Invoke();

        if (respawnDelay <= 0f)
        {
            ReloadLevel();
        }
        else
        {
            Invoke(nameof(ReloadLevel), respawnDelay);
        }
    }

    private void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}