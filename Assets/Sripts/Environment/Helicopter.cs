using UnityEngine;

public class helicopterManager : MonoBehaviour
{
    public GameObject baseHelicopter;
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
            helicopter=baseHelicopter;
            heliSpawned = false;
            reachedTarget = false;
            helicopter = Instantiate(helicopter, transform.position, Quaternion.identity);
            helicopter.transform.SetParent(transform, false);
            helicopter.transform.localPosition = new Vector3(0, 10, -40); 
            helicopter.tag = "Obstacle";
            helicopter.layer = LayerMask.NameToLayer("TransparentFX");
            Animator animator = helicopter.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(helicopter==null)
        {
            helicopter=baseHelicopter;
            heliSpawned = false;
            reachedTarget = false;
            helicopter = Instantiate(helicopter, transform.position, Quaternion.identity);
            helicopter.transform.SetParent(transform, false);
            helicopter.transform.localPosition = new Vector3(0, 10, -40); 
            helicopter.tag = "Obstacle";
            helicopter.layer = LayerMask.NameToLayer("TransparentFX");
            Animator animator = helicopter.GetComponent<Animator>();
        }
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
        barrelb.transform.localScale = new Vector3(100f, 80f, 100f);
        barrelb.transform.SetParent(null);
        Rigidbody rb = barrelb.GetComponent<Rigidbody>();
        if (barrelb.GetComponent<CapsuleCollider>() == null)
        {
        CapsuleCollider capsule = barrelb.AddComponent<CapsuleCollider>();
        capsule.height = .8f;
        capsule.radius = .5f;
        capsule.direction = 1;
        }
        if (rb == null)
        {
            rb = barrelb.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 1f;
        rb.AddForce(Vector3.down * 2500f, ForceMode.Acceleration);
        
        Destroy(barrelb, 9f);
    }
}
