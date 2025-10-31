using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    private float direction = 0f; // Y rotation in degrees
    public GameObject tilePrefab;
    public int poolSize = 30;
    public float tileLength = 30f;
    public Transform player;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private Vector3 nextSpawnPoint = Vector3.zero;

    void Start()
    {
        // initialize pool and spawn initial tiles
        for (int i = 0; i < poolSize; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // check if the backmost tile is behind the player enough to recycle
        GameObject backTile = pool.Peek();
        if (player.position.z - backTile.transform.position.z > tileLength)
        {
            RecycleTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefab, transform);

        // Decide random initial direction for variety
        int chosenDirection = UnityEngine.Random.Range(0, 10);
        if (chosenDirection == 0 && direction > -90) direction -= 30;
        else if (chosenDirection <= 1 && direction < 90) direction += 30;

        tile.transform.position = nextSpawnPoint;
        tile.transform.rotation = Quaternion.Euler(0, direction, 0);

        // Update next spawn point based on this tile's forward direction
        nextSpawnPoint += tile.transform.forward * tileLength;

        pool.Enqueue(tile);
    }

    void RecycleTile()
    {
        GameObject tile = pool.Dequeue();

        // Decide which direction to go next
        int chosenDirection = UnityEngine.Random.Range(0, 10);
        if (chosenDirection == 0 && direction > -90) direction -= 30; // left
        else if (chosenDirection <= 1 && direction < 90) direction += 30; // right
        // straight: direction unchanged

        tile.transform.position = nextSpawnPoint;
        tile.transform.rotation = Quaternion.Euler(0, direction, 0);

        // Update next spawn point
        nextSpawnPoint += tile.transform.forward * tileLength;

        pool.Enqueue(tile);
    }
}