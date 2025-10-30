using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -6f);
    public float followSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}
