using UnityEngine;
using System.Collections.Generic;

public class CloudSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject cloudPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float forwardDistance = 80f;
    public float sideRange = 50f;
    public float yHeight = 25f;

    [Header("Cloud Variation")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.4f);

    [Header("Cleanup")]

    private List<GameObject> activeClouds = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnCloud), 1f, spawnInterval);
    }

    void SpawnCloud()
    {
        Vector3 forward = player.forward.normalized;

        Vector3 pos = player.position +
                      forward * forwardDistance +
                      new Vector3(Random.Range(-sideRange, sideRange), 0, 0);

        pos.y = yHeight;

        GameObject cloud = Instantiate(cloudPrefab, pos, Quaternion.identity);

        float s = Random.Range(scaleRange.x, scaleRange.y);
        cloud.transform.localScale = Vector3.one * s;

        activeClouds.Add(cloud);

    }

    
        
    
}
