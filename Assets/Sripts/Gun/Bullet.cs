using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 80f;      // make this MUCH faster than the car's forwardSpeed
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

        // Ignore potholes completely
        if (other.CompareTag("Pothole"))
        {
            Destroy(gameObject); // bullet still disappears
            return;
        }

        // Destroy barrels and cars
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
        }

        // Destroy bullet on any hit
        Destroy(gameObject);
    }
}

