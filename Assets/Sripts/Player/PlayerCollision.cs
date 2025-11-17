using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Health health;       // drag your Health script here in Inspector
    public int obstacleDamage = 20;  // damage taken when hitting a car/barrel

    void Start()
    {
        if (health == null)
            health = GetComponent<Health>();
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // ignore potholes
        if (other.CompareTag("Pothole"))
            return;

        // collision with destroyable obstacles
        if (other.CompareTag("Obstacle"))
        {
            // apply damage
            health.TakeDamage(obstacleDamage);

            // destroy the obstacle
            Destroy(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If for some reason obstacles use triggers instead of colliders:
        if (other.CompareTag("Obstacle"))
        {
            health.TakeDamage(obstacleDamage);
            Destroy(other.gameObject);
        }
    }
}
