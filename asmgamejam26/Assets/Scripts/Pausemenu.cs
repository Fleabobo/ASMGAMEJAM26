using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("UI Reference")]
    public GameObject pauseMenuPanel;

    [Header("Scenes where pausing is disabled")]
    public string[] disabledInScenes = { "Mainmenu" };

    private bool isPaused = false;
    private Playercontroller playercontroller;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        FindPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        FindPlayer();
    }

    private void FindPlayer()
    {
        playercontroller = FindFirstObjectByType<Playercontroller>();
    }

    private void Update()
    {
        if (IsDisabledInCurrentScene()) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    private bool IsDisabledInCurrentScene()
    {
        string current = SceneManager.GetActiveScene().name;
        foreach (string name in disabledInScenes)
        {
            if (current == name) return true;
        }
        return false;
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (playercontroller != null) playercontroller.inputLocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (playercontroller != null) playercontroller.inputLocked = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Mainmenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}