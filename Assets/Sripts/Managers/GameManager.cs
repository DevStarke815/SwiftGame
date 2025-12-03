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

    [Header("Score Popup UI")]
    public TMP_Text bonusText;            // small "+5", "+50" etc
    public float bonusPopupTime = 0.5f;

    [Header("Difficulty Scaling")]
    [Tooltip("Score needed to increase difficulty by 1 (e.g. 200 = 200, 400, 600)")]
    public float scorePerLevel = 200f;
    [Tooltip("How much to increase player forward speed per level")]
    public float forwardSpeedPerLevel = 1f;
    [Tooltip("Extra hits required per level for obstacles/helicopter")]
    public int extraHitsPerLevel = 1;

    private float score;
    private bool isPlaying = false;       // false until StartGame is pressed

    private float bonusTimer = 0f;
    private int difficultyLevel = 0;
    private float baseForwardSpeed = 0f;

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
        // record base forward speed so we can scale from it
        if (player != null)
        {
            baseForwardSpeed = player.forwardSpeed;
        }

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

        // hide bonus popup initially
        if (bonusText != null)
        {
            bonusText.text = "";
            Color c = bonusText.color;
            c.a = 0f;
            bonusText.color = c;
        }

        difficultyLevel = 0;
        ApplyDifficulty();
    }

    void Update()
    {
        if (isPlaying)
        {
            // time-based score
            score += Time.deltaTime;
            if (scoreText != null)
                scoreText.text = Mathf.FloorToInt(score).ToString();
        }

        // update difficulty based on current score
        UpdateDifficulty();

        // +X popup fade logic
        if (bonusText != null && bonusTimer > 0f)
        {
            bonusTimer -= Time.deltaTime;

            float t = bonusTimer / bonusPopupTime;
            t = Mathf.Clamp01(t);

            Color c = bonusText.color;
            c.a = t;
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

    // Add score from bullets, obstacles, helicopter, etc.
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

        // after adding score, difficulty may change
        UpdateDifficulty();
    }

    // difficulty update based on score (200, 400, 600, ...)
    private void UpdateDifficulty()
    {
        int newLevel = Mathf.FloorToInt(score / scorePerLevel);

        if (newLevel != difficultyLevel)
        {
            difficultyLevel = newLevel;
            ApplyDifficulty();
        }
    }

    private void ApplyDifficulty()
    {
        // increase player speed with difficulty
        if (player != null)
        {
            float newSpeed = baseForwardSpeed + difficultyLevel * forwardSpeedPerLevel;
            player.forwardSpeed = newSpeed;
        }

        // obstacles/helicopters will query GetExtraHitsRequired()
        // so newly spawned ones will require more hits
    }

    // Called by obstacles/helicopter to know how many extra hits they should require
    public int GetExtraHitsRequired()
    {
        return difficultyLevel * extraHitsPerLevel;
    }

    // Quit button on start menu
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
