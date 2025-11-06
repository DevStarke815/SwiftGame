using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    private float direction = 0f; // Y rotation in degrees
    public GameObject tilePrefab;
    public GameObject barrelPrefab;
    public GameObject potholePrefab;
    public GameObject carPrefab;
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
        if ((player.position.z - backTile.transform.position.z > 2*tileLength)||
            Mathf.Abs(player.position.x) - Mathf.Abs(backTile.transform.position.x) > 2*tileLength)
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
        // destroys all obstacles on tile
        for (int i = tile.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(tile.transform.GetChild(i).gameObject);
        }
        // Decide which direction to go next
        int chosenDirection = UnityEngine.Random.Range(0, 10);
        if (chosenDirection == 0 && direction > -90) direction -= 30; // left
        else if (chosenDirection <= 1 && direction < 90) direction += 30; // right
        // straight: direction unchanged

        tile.transform.position = nextSpawnPoint;
        tile.transform.rotation = Quaternion.Euler(0, direction, 0);

        // Update next spawn point
        nextSpawnPoint += tile.transform.forward * tileLength;

        spawnObstacle(tile);
        pool.Enqueue(tile);
    }

    void spawnObstacle(GameObject tile)
    {
        int numberOfObstacles = UnityEngine.Random.Range(1, 6);
        for (int i = 0; i < numberOfObstacles; i++)
        {
            int choseObstacle = UnityEngine.Random.Range(0, 3);
            float zaxis = UnityEngine.Random.Range(-0.45f, .45f);
            float xaxis = UnityEngine.Random.Range(-.45f, .45f);
            if (choseObstacle == 0)
            {
                GameObject barrel = Instantiate(barrelPrefab, tile.transform);
                barrel.transform.localScale = new Vector3(.05f,.5f,.03f);
                barrel.transform.localPosition = new Vector3(
                xaxis, 1f , zaxis);
            }
            if (choseObstacle == 1)
            {
                GameObject pothole = Instantiate(potholePrefab, tile.transform);
                pothole.transform.localScale = new Vector3(.1f,.06f,.01f);
                pothole.transform.localPosition = new Vector3(
                xaxis, .51f , zaxis);
            }
            if (choseObstacle == 2)
            {
                GameObject car = Instantiate(carPrefab, tile.transform);
                car.transform.localScale = new Vector3(1f/15f,1f,1f/30f);
                car.transform.localPosition = new Vector3(
                xaxis, 1f , zaxis);
            }
        }        
    }
}