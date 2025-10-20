using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float pushRange = 2f;
    public float moveThreshold = 0.05f; //prevents flickering between Idle/Walk

    [Header("Animation")]
    public Animator animator;
    public string idleAnim = "Idle";
    public string walkAnim = "Walk";
    public string pushAnim = "Push";

    [Header("Push Settings")]
    public LayerMask pushableLayer;

    private Rigidbody rb;
    private bool isPushing = false;
    private bool isWalking = false;
    private Transform pushTarget;
    private Vector3 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //prevent player from rotating physically

        if (animator == null)
            Debug.LogWarning("No Animator assigned to PlayerMovement");
    }

    private void Update()
    {
        if (!isPushing)
            ReadMovementInput();

        CheckForPushable();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void ReadMovementInput()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Simple WASD controls
        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        moveInput = new Vector3(moveX, 0, moveZ).normalized;
    }

    private void HandleMovement()
    {
        if (isPushing) return;

        if (moveInput.magnitude >= moveThreshold)
        {
            // Move relative to camera direction
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * moveInput.z + camRight * moveInput.x;

            // Move player (no rotation)
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            // Animation control
            if (!isWalking)
            {
                animator.CrossFade(walkAnim, 0.1f);
                isWalking = true;
            }
        }
        else
        {
            if (isWalking)
            {
                animator.CrossFade(idleAnim, 0.1f);
                isWalking = false;
            }
        }
    }

    private void CheckForPushable()
    {
        //detect objects with "Push" tag within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRange, pushableLayer);

        if (hitColliders.Length > 0)
        {
            pushTarget = hitColliders[0].transform;

            //press and hold E to push
            if (Input.GetKey(KeyCode.E))
            {
                StartPush();
            }
            else if (isPushing)
            {
                StopPush();
            }
        }
        else
        {
            if (isPushing)
                StopPush();
        }
    }

    private void StartPush()
    {
        if (isPushing) return;

        isPushing = true;
        animator.Play(pushAnim);

        //optional: apply small forward force to simulate push motion
        if (pushTarget != null)
        {
            Rigidbody targetRb = pushTarget.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                Vector3 pushDir = (pushTarget.position - transform.position).normalized;
                targetRb.AddForce(pushDir * 5f, ForceMode.Impulse);
            }
        }
    }

    private void StopPush()
    {
        isPushing = false;
        animator.Play(idleAnim);
    }

    private void OnDrawGizmosSelected()
    {
        //visualize push detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pushRange);
    }
}