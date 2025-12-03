using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 80f;
    public float lifetime = 3f;

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
        // Ignore the player
        if (other.CompareTag("Player"))
            return;

        // Potholes cannot be destroyed
        if (other.CompareTag("Pothole"))
        {
            Destroy(gameObject);
            return;
        }

        // === Helicopter (for future use) ===
        // When the helicopter enemy exists:
        // - Tag it "Helicopter"
        // - Then this will work automatically
        //if (other.CompareTag("Helicopter"))
        //{
            //GameManager.Instance.AddScore(50);   // helicopter is worth +50
            //Destroy(other.gameObject);
            //Destroy(gameObject);
            //return;
        //}

        // === Destroyable obstacles (barrels, cars) ===
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.AddScore(5);    // standard obstacle score
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        // === Everything else ===
        Destroy(gameObject);
    }
}
