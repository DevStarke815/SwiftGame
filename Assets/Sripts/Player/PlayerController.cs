using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 6f;
    public float jumpForce = 7f;
    public float gravity = -20f;

    [Header("Ground")]
    public float groundY = 0f; 
    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        HandleInput();
        ApplyGravity();
        MovePlayer();
    }

    void HandleInput()
    {
        // Jump using Space or left mouse button 
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // simple ground clamp (player pivot is center of cube at y = 0.5)
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
        // forward movement on Z
        Vector3 forwardMove = Vector3.forward * forwardSpeed * Time.deltaTime;
        Vector3 verticalMove = velocity * Time.deltaTime; 
        transform.Translate(forwardMove + verticalMove, Space.World);
    }
}
