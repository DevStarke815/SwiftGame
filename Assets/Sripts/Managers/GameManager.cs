using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   // singleton-style access

    [Header("References")]
    public PlayerController player;
    public TMP_Text scoreText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;      // GameOverPanel object
    public TMP_Text finalScoreText;       // optional score text on game over

    [Header("Start UI")]
    public GameObject startPanel;         // StartPanel object

    private float score;
    private bool isPlaying = false;       // false until StartGame is pressed

    [Header("Score Popup UI")]
    public TMP_Text bonusText;            // reference to +5 / +50 text
    public float bonusPopupTime = 0.5f;   // how long the popup stays visible

    private float bonusTimer = 0f;        // internal timer for fading


    void Awake()
    {
        // simple singleton pattern; assumes one GameManager in the scene
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start in "waiting to start" state
        isPlaying = false;

        // Show start panel, hide game over panel
        if (startPanel != null)
            startPanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Pause time until the player presses Start
        Time.timeScale = 0f;

        // Reset score display
        score = 0f;
        if (scoreText != null)
            scoreText.text = "0";
    }

    void Update()
    {
        if (isPlaying)
        {
            score += Time.deltaTime;
            if (scoreText != null)
                scoreText.text = Mathf.FloorToInt(score).ToString();
        }

        // +5 popup fade logic
        if (bonusText != null && bonusTimer > 0f)
        {
            bonusTimer -= Time.deltaTime; // use scaled time

            // t = 1 -> opaque, t = 0 -> fully invisible
            float t = bonusTimer / bonusPopupTime;

            Color c = bonusText.color;
            c.a = Mathf.Clamp01(t);
            bonusText.color = c;

            if (bonusTimer <= 0f)
            {
                bonusText.text = "";
            }
        }
    }

    // Called by Start button
    public void StartGame()
    {
        if (isPlaying) return;

        isPlaying = true;

        // Hide start panel
        if (startPanel != null)
            startPanel.SetActive(false);

        // Resume time
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (!isPlaying) return;

        isPlaying = false;

        // Stop time
        Time.timeScale = 0f;

        // Show game over panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Show final score if desired
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    // NEW: add score from things like shooting obstacles
    public void AddScore(int amount)
    {
        score += amount;

        if (scoreText != null)
            scoreText.text = Mathf.FloorToInt(score).ToString();

        // show +amount popup
        if (bonusText != null && amount > 0)
        {
            bonusText.text = "+" + amount;

            Color c = bonusText.color;
            c.a = 1f;        // fully visible immediately
            bonusText.color = c;

            bonusTimer = bonusPopupTime;  // restart fade timer
        }
    }

}