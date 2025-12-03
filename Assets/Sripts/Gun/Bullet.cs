using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 80f;      // much faster than the car's forwardSpeed
    public float lifetime = 3f;    // seconds before auto-destroy

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Skip hitting the player
        if (other.CompareTag("Player"))
            return;

        // Potholes should not be destroyable
        if (other.CompareTag("Pothole"))
        {
            Destroy(gameObject);   // bullet disappears, pothole stays
            return;
        }

        // Check for Destructible on whatever we hit (obstacle, helicopter, etc.)
        Destructible destructible = other.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.ApplyHit();
            Destroy(gameObject);
            return;
        }

        // Everything else (ground, walls, etc.): just destroy the bullet
        Destroy(gameObject);
    }
}