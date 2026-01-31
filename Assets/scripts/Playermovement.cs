using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Vector2 input;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (groundCheck == null)
        {
            // Create a ground check point if not assigned
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.parent = transform;
            gc.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = gc.transform;
        }
    }

    void Update()
    {
        // Get input for movement (top-down: WASD/arrow keys)
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump input (only if grounded)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset Y velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Move the player (top-down: X and Y axes)
        Vector2 move = new Vector2(input.x, input.y).normalized * moveSpeed;
        rb.linearVelocity = new Vector2(move.x, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
