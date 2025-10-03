using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    [Header("References")]
    public Animator animator;

    private Rigidbody rb;
    private bool isClimbing = false;
    private bool nearClimbable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure starting conditions are correct
        isClimbing = false;
        nearClimbable = false;
        rb.useGravity = false;
    }

    void Update()
    {
        // Handle walking or climbing movement
        if (!isClimbing)
        {
            HandleMovement();
        }
        else
        {
            HandleClimb();
        }

        // Press E to climb if near a climbable
        if (nearClimbable && !isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            StartClimbing();
        }

        // Press E again to stop climbing manually
        if (isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            StopClimbing();
        }
    }

    // FORWARD/BACKWARD MOVEMENT (A = back, D = forward)
    void HandleMovement()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            move = -1f; // backward
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move = 1f;  // forward
        }

        Vector3 moveDirection = transform.forward * move;

        // Maintain gravity�s Y velocity
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Handle animation
        bool isWalking = move != 0;
        animator.SetBool("isWalking", isWalking);
    }

    // CLIMB MOVEMENT
    void HandleClimb()
    {
        // Move upward while climbing
        rb.linearVelocity = new Vector3(0, climbSpeed, 0);
    }

    void StartClimbing()
    {
        // Only climb if we're near a climbable surface
        if (nearClimbable && !isClimbing)
        {
            isClimbing = true;
            rb.useGravity = false; // Disable gravity while climbing
            rb.linearVelocity = Vector3.zero;

            // Play climbing animation
            animator.SetBool("isClimbing", true);

            Debug.Log("Started climbing");
        }
    }

    void StopClimbing()
    {
        if (isClimbing)
        {
            isClimbing = false;
            rb.useGravity = true; // Re-enable gravity
            animator.SetBool("isClimbing", false);

            Debug.Log("Stopped climbing");
        }
    }

    // Detect climbable areas
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("climbable"))
        {
            nearClimbable = true;
            Debug.Log("Near climbable");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("climbable"))
        {
            nearClimbable = false;
            StopClimbing();
            Debug.Log("Left climbable area");
        }
    }
}