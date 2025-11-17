using UnityEngine;

public class helicopterManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public GameObject helicopterPrefab;
    public GameObject bomb;
    public GameObject helicopter;
    public bool reachedTarget = false;
    private Vector3 targetPosition;
    public Transform playerTransform;
    public float bombtimer = 3f;
    public float heliTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = PlayerController.getPlayerTransform();
        helicopter = Instantiate(helicopterPrefab, playerTransform.position, Quaternion.identity);
        helicopter.transform.SetParent(playerTransform, false);
        helicopter.transform.position = new Vector3(0, 10, -40); 



    }

    // Update is called once per frame
    void Update()
    {
        heliTimer-=Time.deltaTime;
        if(heliTimer<=0)
        {

        if (helicopter == null) return;

        playerTransform = PlayerController.getPlayerTransform();
        targetPosition = playerTransform.position + playerTransform.forward * 5f + Vector3.up * 5f;
        if (!reachedTarget)
        {
            helicopter.transform.position = Vector3.MoveTowards(
                helicopter.transform.position,
                targetPosition,
                5f * Time.deltaTime
            );
            if (Vector3.Distance(helicopter.transform.position, targetPosition) < 0.01f)
            {
                reachedTarget = true;
            }
        }
        else
        {
            bombtimer-= Time.deltaTime;
            Vector3 followPosition = playerTransform.position + playerTransform.forward * 10f + Vector3.up * 5f;
            helicopter.transform.position = Vector3.Lerp(
                helicopter.transform.position,
                followPosition,
                Time.deltaTime * 5f
            );
            if(bombtimer==0)
            {
                dropbomb();
            }
        }
        }

    }
    void dropbomb()
    {
        
    }
}
