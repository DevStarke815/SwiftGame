using UnityEngine;

public class helicopterManager : MonoBehaviour
{
    public GameObject helicopterPrefab;
    public GameObject barrelprefab;
    public GameObject helicopter;
    public GameObject barrelb;
    public bool reachedTarget = false;
    public bool heliSpawned = false;
    private Vector3 targetPosition;
    public float barreltimer = 3f;
    public float heliTimer = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        helicopter = Instantiate(helicopterPrefab, transform.position, Quaternion.identity);
        helicopter.transform.SetParent(transform, false);
        helicopter.transform.position = new Vector3(0, 10, -40); 
    }

    // Update is called once per frame
    void Update()
    {
        heliTimer-=Time.deltaTime;
        if(heliTimer<=0)
        {
        int rand = Random.Range(0, 10);
        if(rand<1)
        {
            heliSpawned = true;
        }
        else
        {
            heliTimer=10f;
        }
        }
        if(heliSpawned)
        {
        targetPosition = transform.position + transform.forward * 20f + Vector3.up * 5f;
        if (!reachedTarget)
        {
            helicopter.transform.position = Vector3.MoveTowards(
                helicopter.transform.position,
                targetPosition,
                10f * Time.deltaTime
            );
            if (Vector3.Distance(helicopter.transform.position, targetPosition) < 0.01f)
            {
                reachedTarget = true;
            }
        }
        else
        {
            barreltimer-= Time.deltaTime;
            Vector3 followPosition = transform.position + transform.forward * 20f + Vector3.up * 5f;
            helicopter.transform.position = Vector3.Lerp(
                helicopter.transform.position,
                followPosition,
                Time.deltaTime * 5f
            );
            if(barreltimer<=0)
            {
                barreltimer=3;
                dropbarrel();
            }
        }
        }
    }
    void dropbarrel()
    {
        barrelb = Instantiate(barrelprefab,helicopter.transform.position,  Quaternion.Euler(0f,0f,0f));
        barrelb.transform.SetParent(helicopter.transform);
        barrelb.transform.localScale = new Vector3(1f, .8f, 1f);
        barrelb.transform.SetParent(null);
        Destroy(barrelb, 9f);
    }
}
