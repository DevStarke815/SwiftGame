using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public TMP_Text scoreText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;      // assign GameOverPanel here
    public TMP_Text finalScoreText;       // optional: text on panel that shows score

    private float score;
    private bool isPlaying = true;

    void Start()
    {
        // make sure panel is hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isPlaying) return;

        score += Time.deltaTime;
        if (scoreText != null)
            scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    public void GameOver()
    {
        if (!isPlaying) return;  // prevent double calls

        isPlaying = false;

        // stop time so player/car stops moving
        Time.timeScale = 0f;

        // show game over UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // show final score on the panel if you hooked it up
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
}
