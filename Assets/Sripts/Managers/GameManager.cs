using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
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
        if (!isPlaying) return;

        score += Time.deltaTime;

        if (scoreText != null)
            scoreText.text = Mathf.FloorToInt(score).ToString();
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

    public void QuitGame()
    {
        // In a built game this will close the application.
        Application.Quit();

        // This makes Quit work inside the Unity Editor for testing.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}