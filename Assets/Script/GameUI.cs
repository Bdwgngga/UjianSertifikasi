using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Pause Menu Settings")]
    [SerializeField] private GameObject pauseMenu; // Assign GameObject untuk Pause Menu
    private bool isPaused = false;
    public static GameUI Instance;

    [Header("Score Settings")]
    [SerializeField] private TMP_Text scoreText; //UI Score
    private int currentScore = 0;
    private PlayerInputs playerInputs;

    [Header("Timer Settings")]
    [SerializeField] private float startTime = 60f; // Waktu mulai dalam detik
    private float currentTime;

    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverPanel;
    private bool isGameOver = false;


    [Header("UI Timer Display")]
    [SerializeField] private TMP_Text timerText;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerInputs = new PlayerInputs();
        playerInputs.BasedMoveSet.Pause.performed += _ => TogglePause();
        audioSource.mute = false;

        // Singleton untuk memastikan hanya satu instance

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();

        currentTime = startTime;

        // Pastikan GameOver Panel nonaktif saat game dimulai
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isGameOver)
        {
            CountdownTimer();
        }
    }

    public void OnEnable()
    {
        playerInputs.Enable();
    }

    public void OnDisable()
    {
        playerInputs.Disable();
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // Toggle nilai isPaused
        pauseMenu.SetActive(isPaused); // Aktifkan atau nonaktifkan Pause Menu
        audioSource.mute = (isPaused);
        Time.timeScale = isPaused ? 0 : 1; // Pause atau resume waktu permainan
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        audioSource.mute = false;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        audioSource.mute = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
        audioSource.mute = false;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public void CountdownTimer()
    {
        // Kurangi waktu
        currentTime -= Time.deltaTime;
        Debug.Log("Current Time: " + currentTime);
        // Update UI Timer
        if (timerText != null)
        {

            timerText.text = "Timer: " + Mathf.Ceil(currentTime).ToString(); // Tampilkan angka bulat
        }

        // Jika waktu habis
        if (currentTime <= 0f)
        {
            currentTime = 0f; // Pastikan tidak minus
            Time.timeScale = 0;
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // Aktifkan panel GameOver
        if (gameOverPanel != null)
        {
            audioSource.mute = true;
            gameOverPanel.SetActive(true);
        }

        Debug.Log("Game Over!");
    }
}
