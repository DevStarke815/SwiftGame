using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Forward Motion")]
    [Tooltip("Constant forward speed (units/second)")]
    public float forwardSpeed = 10f;

    [Header("Steering (car-like, no auto-center)")]
    [Tooltip("Maximum yaw rate at full input (degrees/second)")]
    public float maxSteerRate = 120f;
    [Tooltip("How quickly the steer rate ramps to the target (degrees/second^2)")]
    public float steerAcceleration = 600f;
    [Tooltip("Optional friction on steering rate when you release (0 = none)")]
    public float steerFriction = 0f;

    [Header("Jump / Gravity (optional)")]
    public bool enableJump = false;
    public float jumpForce = 7f;
    public float gravity = -20f;
    [Tooltip("Distance to check for ground below player")]
    public float groundCheckDistance = 0.6f;
    [Tooltip("Layer mask for what counts as ground")]
    public LayerMask groundLayer = ~0; // everything by default

    [Header("Fall Detection")]
    [Tooltip("Y position below which player dies")]
    public float fallThreshold = -10f;

    [Header("Visual Tilt (purely cosmetic)")]
    public float rollPerDegPerSec = 0.06f; // roll (deg) per deg/s of steer rate
    public float visualLerp = 10f;

    // internals
    private float steerRate;   // current yaw rate (deg/s)
    private float yaw;         // current yaw angle in degrees
    private Vector3 velocity;  // only Y used if jumping
    private bool isGrounded;
    private BoxCollider boxCollider;
    private bool hasDied = false; // prevent multiple death calls

    void Start()
    {
        yaw = transform.eulerAngles.y;
        isGrounded = true;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // 1) Steering input
        float input = Input.GetAxisRaw("Horizontal"); // A/D or arrows (-1..1)
        float targetRate = input * maxSteerRate;

        // accelerate steerRate toward targetRate
        steerRate = Mathf.MoveTowards(steerRate, targetRate, steerAcceleration * dt);

        // optional friction when no input
        if (Mathf.Approximately(input, 0f) && steerFriction > 0f)
            steerRate = Mathf.MoveTowards(steerRate, 0f, steerFriction * dt);

        // integrate yaw
        yaw += steerRate * dt;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // 2) Constant forward motion
        transform.position += transform.forward * forwardSpeed * dt;

        // 3) Optional jump/gravity
        if (enableJump)
            HandleJumpAndGravity(dt);

        // 4) Visual tilt based on steering rate (lean into turns)
        ApplyVisualTilt(dt);

        // 5) Check if player has fallen too far
        if (!hasDied && transform.position.y < fallThreshold)
        {
            hasDied = true; // prevent multiple death triggers
            Health healthComponent = GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(healthComponent.currentHealth); // instant death
            }
        }
    }

    void HandleJumpAndGravity(float dt)
    {
        // Check if there's ground beneath us
        isGrounded = CheckGrounded();

        // Space only for jump now
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }

        // Apply gravity when not grounded
        if (!isGrounded)
        {
            velocity.y += gravity * dt;
            transform.position += Vector3.up * (velocity.y * dt);
        }
        else
        {
            // Reset vertical velocity when grounded
            velocity.y = 0f;
        }
    }

    bool CheckGrounded()
    {
        // Raycast downward from the center of the collider
        Vector3 origin = transform.position;
        
        // Cast a ray slightly longer than half the collider height
        if (Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer))
        {
            return true;
        }
        
        return false;
    }

    void ApplyVisualTilt(float dt)
    {
        // roll is proportional to how fast you are turning
        float targetRoll = -steerRate * rollPerDegPerSec; // lean into turn
        Quaternion baseYaw = Quaternion.Euler(0f, yaw, 0f);
        Quaternion roll = Quaternion.Euler(0f, 0f, targetRoll);
        Quaternion targetRot = baseYaw * roll;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, visualLerp * dt);
    }
    
    public Transform getPlayerTransform()
    {
        return transform;
    }

    // Optional: Visualize the ground check ray in the editor
    
    
}