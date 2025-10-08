using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;     // empty วางใต้ปลาย collider เล็กน้อย
    public float groundRayLength = 0.08f; // ระยะสั้นๆพอ (5–8 ซม. ในหน่วย world)
    public LayerMask groundLayer;     // ใส่เฉพาะชั้น "Ground"

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    // input buffer เล็กน้อยให้ลื่นขึ้น (กดก่อนลงพื้นนิดเดียวแล้วติด)
    private bool jumpQueued;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float moveInput = Input.GetAxisRaw("Horizontal");

        _cachedMove = moveInput;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpQueued = true;
            jumpBufferCounter = jumpBufferTime;
        }

        if (jumpQueued)
        {
            jumpBufferCounter -= Time.deltaTime;
            if (jumpBufferCounter <= 0f) jumpQueued = false;
        }


        // พลิกตัว
        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();
    }

    private float _cachedMove;

    void FixedUpdate()
    {
        // เช็กพื้นด้วย Raycast ลงจากจุด groundCheck
        // ให้แน่ใจว่า groundCheck ต่ำกว่าขอบล่างของ collider ~0.02–0.05
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundRayLength, groundLayer);

        // เดิน
        rb.velocity = new Vector2(_cachedMove * moveSpeed, rb.velocity.y);

        // กระโดด
        if (jumpQueued)
        {
            // reset ความเร็วแกน Y กัน “กดตอนตก” แล้วแรงรวมแปลกๆ
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpQueued = false;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(groundCheck.position,
                            groundCheck.position + Vector3.down * groundRayLength);
        }
    }
}
