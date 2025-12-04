using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Health health;      
    public int obstacleDamage = 20;  // damage from cars/barrels
    public int potholeDamage = 10;   // damage from potholes
    
    void Start()
    {
        if (health == null)
            health = GetComponent<Health>();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        
        // Handle pothole collision - damage but don't destroy
        if (other.CompareTag("Pothole"))
        {
            health.TakeDamage(potholeDamage);
            return;
        }
        
        // Handle destroyable obstacles
        if (other.CompareTag("Obstacle"))
        {
            health.TakeDamage(obstacleDamage);
            Destroy(other);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Pothole as trigger
        if (other.CompareTag("Pothole"))
        {
            health.TakeDamage(potholeDamage);
            return;
        }
        
        // Obstacle as trigger
        if (other.CompareTag("Obstacle"))
        {
            health.TakeDamage(obstacleDamage);
            Destroy(other.gameObject);
        }
    }
}