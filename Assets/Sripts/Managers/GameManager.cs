using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public TMP_Text scoreText;
    private float score;
    private bool isPlaying = true;

    void Update()
    {
        if (!isPlaying) return;

        score += Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    public void GameOver()
    {
        isPlaying = false;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
}
