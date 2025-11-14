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

        // destroy obstacles / enemies by tag
        //if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
       // {
           // Destroy(other.gameObject);
        //}

        // Destroy the bullet on any hit (you can tighten this later)
        Destroy(gameObject);
    }
}

