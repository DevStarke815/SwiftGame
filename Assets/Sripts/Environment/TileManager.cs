using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    private float direction = 0;
    public GameObject tilePrefab;
    public int poolSize = 30;
    public float tileLength = 30f;
    public Transform player;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private float nextSpawnZ = 0f;
    private float nextSpawnX = 0f;

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
        nextSpawnZ += tileLength*1.5f;
    }

    void RecycleTile()
    {
        GameObject tile = pool.Dequeue();
        // This will decide which direction to go next
        int chosenDirection = Random.Range(0,10);
        //left turn
        if((chosenDirection<=1)&&(direction>-90))
        {
            tile.transform.position = new Vector3(nextSpawnX, -0.5f, nextSpawnZ);
            tile.transform.rotation = Quaternion.Euler(0, direction-30, 0);
            direction-=30;
            nextSpawnZ += tileLength*1.5f;
        }
        //turn right
        else if((chosenDirection<=4)&&(direction<90))
        {
            tile.transform.position = new Vector3(nextSpawnX, -0.5f, nextSpawnZ);
            tile.transform.rotation = Quaternion.Euler(0, direction+30, 0);
            direction+=30;
            nextSpawnZ += tileLength*1.5f;
        }
        //straight
        else
        {
            tile.transform.position = new Vector3(nextSpawnX, -0.5f, nextSpawnZ);
            tile.transform.rotation = Quaternion.Euler(0, direction, 0);
            nextSpawnZ += tileLength*1.5f;
        }
        pool.Enqueue(tile);
    }


}
