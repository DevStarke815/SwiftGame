using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int poolSize = 6;
    public float tileLength = 20f;
    public Transform player;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private float nextSpawnZ = 0f;

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
        tile.transform.position = new Vector3(0f, -0.5f, nextSpawnZ);
        pool.Enqueue(tile);
        nextSpawnZ += tileLength;
    }

    void RecycleTile()
    {
        GameObject tile = pool.Dequeue();
        tile.transform.position = new Vector3(0f, -0.5f, nextSpawnZ);
        nextSpawnZ += tileLength;
        pool.Enqueue(tile);
    }
}
