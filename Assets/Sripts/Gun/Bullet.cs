using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 80f;      // much faster than the car's forwardSpeed
    public float lifetime = 3f;    // seconds before auto-destroy

    void Start()
    {
        // Auto-destroy even if nothing is hit
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move along our own forward direction
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Skip hitting the player
        if (other.CompareTag("Player"))
            return;

        // Potholes should NOT be destroyed; bullet still disappears
        if (other.CompareTag("Pothole"))
        {
            Destroy(gameObject);
            return;
        }

        // Destroy barrels and cars (things tagged as "Obstacle")
        if (other.CompareTag("Obstacle"))
        {
            // award score when we destroy an obstacle
            if (GameManager.Instance != null)
            {
                // tweak this value to whatever you want per obstacle
                GameManager.Instance.AddScore(10);
            }

            Destroy(other.gameObject);   // destroy the obstacle
            Destroy(gameObject);         // destroy the bullet
            return;
        }

        // Any other collision (ground, walls, etc.): just destroy the bullet
        Destroy(gameObject);
    }
}