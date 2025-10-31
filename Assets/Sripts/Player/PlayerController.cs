using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Forward")]
    public float forwardSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float groundY = 0f;

    [Header("Steering (car-like)")]
    public float lateralAccel = 20f;        // how hard input accelerates sideways
    public float lateralDrag = 12f;         // slows lateral velocity when not steering
    public float lateralMaxSpeed = 6f;      // cap side speed so you can’t whip around
    public float autoCenterStrength = 0.5f; // gentle pull back toward X = 0
    public float roadHalfWidth = 3.5f;      // soft wall at ±this X
    public float boundarySpring = 35f;      // push back when you hit the soft wall
    public float boundaryDamping = 8f;      // damp bouncing at the wall

    [Header("Visual Swerve")]
    public float yawPerSideSpeed = 3f;      // degrees of yaw per unit side speed
    public float rollPerSideSpeed = 8f;     // degrees of roll per unit side speed
    public float visualTurnLerp = 8f;       // how fast visuals catch up

    // internals
    private Vector3 velocity;               // only Y is used for jump/gravity
    private float sideVel;                  // lateral velocity (X)
    private bool isGrounded;

    void Update()
    {
        HandleInput();
        ApplyGravity();
        MovePlayer();
        ApplyVisualSwerve();
    }

    void HandleInput()
    {
        // Jump
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }

        // Car-like lateral input
        float h = Input.GetAxisRaw("Horizontal"); // -1..1
        float dt = Time.deltaTime;

        // 1) Input adds sideways acceleration
        sideVel += h * lateralAccel * dt;

        // 2) Gentle auto-center toward X=0
        sideVel += -transform.position.x * autoCenterStrength * dt;

        // 3) Drag (reduces side velocity when you stop steering)
        sideVel = Mathf.MoveTowards(sideVel, 0f, lateralDrag * dt);

        // 4) Cap the lateral speed
        sideVel = Mathf.Clamp(sideVel, -lateralMaxSpeed, lateralMaxSpeed);

        // 5) Soft boundary spring (keeps you inside ±roadHalfWidth)
        float x = transform.position.x;
        float over = Mathf.Abs(x) - roadHalfWidth;
        if (over > 0f)
        {
            // spring force pushes you back toward the road, plus damping against current sideVel
            float dir = Mathf.Sign(x); // +1 if right side, -1 if left
            float springAccel = -(dir * boundarySpring * over) - (boundaryDamping * sideVel);
            sideVel += springAccel * dt;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;

        float nextY = transform.position.y + velocity.y * Time.deltaTime;
        if (nextY <= groundY + 0.5f)
        {
            transform.position = new Vector3(transform.position.x, groundY + 0.5f, transform.position.z);
            velocity.y = 0f;
            isGrounded = true;
        }
    }

    void MovePlayer()
    {
        float dt = Time.deltaTime;

        // forward motion
        Vector3 forwardMove = Vector3.forward * forwardSpeed * dt;

        // lateral motion from our car-like model
        Vector3 sideMove = Vector3.right * sideVel * dt;

        // vertical motion from jump/gravity
        Vector3 verticalMove = velocity * dt;

        transform.Translate(forwardMove + sideMove + verticalMove, Space.World);
    }

    void ApplyVisualSwerve()
    {
        // tilt the cube a bit when it’s sliding sideways (purely visual)
        float targetYaw = sideVel * yawPerSideSpeed;   // yaw around Y (nose pointing slightly)
        float targetRoll = -sideVel * rollPerSideSpeed; // roll around Z (lean into turn)

        Quaternion targetRot = Quaternion.Euler(0f, targetYaw, targetRoll);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, visualTurnLerp * Time.deltaTime);
    }
}