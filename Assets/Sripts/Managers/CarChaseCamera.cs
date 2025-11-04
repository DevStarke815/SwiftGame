using UnityEngine;

public class CarChaseCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                  // drag your Player here

    [Header("Framing")]
    public Vector3 localOffset = new Vector3(0f, 5f, -10f); // car-local offset (back & up)
    public float lookAhead = 7f;             // how far ahead of car to look

    [Header("Smoothness")]
    public float yawLag = 0.35f;             // seconds to catch up to car yaw (up = slower pan)
    public float positionSmoothTime = 0.20f; // seconds to smooth camera position
    public float focusSmoothTime = 0.15f;    // seconds to smooth the focus (car position)
    public float extraRotLerp = 6f;          // small extra smoothing for rotation

    [Header("Up Vector")]
    [Range(0f, 1f)] public float upLock = 1f; // 1 = keep horizon level (world up), 0 = follow car roll

    [Header("Collision (optional)")]
    public bool avoidClipping = false;
    public LayerMask clipMask;
    public float clipRadius = 0.25f;
    public float clipBuffer = 0.1f;

    // internals
    private float smoothedYaw;
    private float yawVel;                    // ref velocity for SmoothDampAngle
    private Vector3 posVel;                  // ref velocity for SmoothDamp (position)
    private Vector3 focusVel;                // ref velocity for SmoothDamp (focus point)
    private Vector3 focus;                   // lagged focus point (car position)

    void Start()
    {
        if (target)
        {
            smoothedYaw = target.eulerAngles.y;
            focus = target.position;
            transform.position = target.TransformPoint(localOffset);
            transform.rotation = Quaternion.LookRotation(target.forward, Vector3.up);
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // 1) Smooth the focus point so we don't hard-snap to the car's position
        focus = Vector3.SmoothDamp(focus, target.position, ref focusVel, focusSmoothTime);

        // 2) Smooth the yaw so camera pans slower than car turning
        float targetYaw = target.eulerAngles.y;
        smoothedYaw = Mathf.SmoothDampAngle(smoothedYaw, targetYaw, ref yawVel, yawLag);

        // Build a yaw-only rotation (keeps horizon stable); we’ll blend up vectors below
        Quaternion yawRot = Quaternion.Euler(0f, smoothedYaw, 0f);

        // 3) Desired camera position from lagged yaw & lagged focus
        Vector3 desiredPos = focus + yawRot * localOffset;

        // Optional obstacle avoidance
        if (avoidClipping)
        {
            Vector3 pivot = focus;
            Vector3 toCam = desiredPos - pivot;
            float dist = toCam.magnitude;
            if (Physics.SphereCast(pivot, clipRadius, toCam.normalized, out RaycastHit hit, dist, clipMask, QueryTriggerInteraction.Ignore))
            {
                desiredPos = hit.point - toCam.normalized * clipBuffer;
            }
        }

        // 4) Smooth position into place
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref posVel, positionSmoothTime);

        // 5) Build a stable up vector (blend world up vs car up)
        Vector3 camUp = Vector3.Slerp(Vector3.up, target.up, Mathf.Clamp01(1f - upLock)); // upLock=1 -> world up

        // 6) Look slightly ahead using the same lagged yaw (so look direction also lags)
        Vector3 smoothedForward = yawRot * Vector3.forward;
        Vector3 lookTarget = focus + smoothedForward * lookAhead;

        Quaternion targetRot = Quaternion.LookRotation((lookTarget - transform.position).normalized, camUp);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Mathf.Clamp01(extraRotLerp * Time.deltaTime));
    }
}

