using UnityEngine;

public class CarGun : MonoBehaviour
{
    [Header("References")]
    public Transform muzzle;              // front / middle of the car
    public GameObject bulletPrefab;       // bullet prefab with Bullet script

    [Header("Fire Settings")]
    public float fireRate = 15f;          // bullets per second
    public float maxAimDistance = 500f;
    public LayerMask aimMask = ~0;        // layers we can aim at (default: everything)

    private float fireCooldown;

    void Update()
    {
        if (muzzle == null || bulletPrefab == null)
            return;

        fireCooldown -= Time.deltaTime;

        // Left mouse held -> continuous fire
        if (Input.GetMouseButton(0))
        {
            if (fireCooldown <= 0f)
            {
                FireOnce();
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void FireOnce()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Ray from the active camera through the mouse
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // We'll just shoot *along the ray direction*.
        // That guarantees bullets follow the mouse, even if nothing is hit.
        Vector3 shootDir = ray.direction;

        // Optionally, try to aim at a hit point for debugging / future use
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
        {
            shootDir = (hit.point - muzzle.position).normalized;
        }

        Quaternion rot = Quaternion.LookRotation(shootDir, Vector3.up);
        Instantiate(bulletPrefab, muzzle.position, rot);
    }
}
