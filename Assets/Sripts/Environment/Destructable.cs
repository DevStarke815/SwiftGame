using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Health")]
    [Tooltip("Base number of hits needed to destroy at difficulty level 0")]
    public int baseHitsToDestroy = 1;

    [Header("Scoring")]
    [Tooltip("Points awarded when this object is destroyed")]
    public int scoreValue = 5;   // set 5 for obstacles, 50 for helicopter later

    private int hitsRemaining;

    void Start()
    {
        int extraHits = 0;
        if (GameManager.Instance != null)
        {
            extraHits = GameManager.Instance.GetExtraHitsRequired();
        }

        hitsRemaining = baseHitsToDestroy + extraHits;
    }

    // Bullet calls this when it hits
    public void ApplyHit()
    {
        hitsRemaining--;

        if (hitsRemaining <= 0)
        {
            if (GameManager.Instance != null && scoreValue > 0)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            Destroy(gameObject);
        }
    }
}
