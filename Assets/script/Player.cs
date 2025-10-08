using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;     // empty �ҧ����� collider ��硹���
    public float groundRayLength = 0.08f; // ��������� (5�8 ��. �˹��� world)
    public LayerMask groundLayer;     // ���੾�Ъ�� "Ground"

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    // input buffer ��硹��������蹢�� (����͹ŧ��鹹Դ�������ǵԴ)
    private bool jumpQueued;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public int Hp;
    public int FullHp = 100;

    public TextMeshProUGUI textHp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Hp = 100;
    }

    void Update()
    {
        textHp.text = $"HP {Hp} / {FullHp}";
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

        if (Hp <= 0)
        {
            resetHp();
        }

        // ��ԡ���
        if (moveInput > 0 && !isFacingRight) 
        { 
            Flip(); 
        }

        else if (moveInput < 0 && isFacingRight) 
        {
            Flip(); 
        }
    }

    private float _cachedMove;

    void FixedUpdate()
    {
        // �硾�鹴��� Raycast ŧ�ҡ�ش groundCheck
        // ��������� groundCheck ��ӡ��Ңͺ��ҧ�ͧ collider ~0.02�0.05
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundRayLength, groundLayer);

        // �Թ
        rb.velocity = new Vector2(_cachedMove * moveSpeed, rb.velocity.y);

        // ���ⴴ
        if (jumpQueued)
        {
            // reset ��������᡹ Y �ѹ ����͹��� �����ç����š�
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpQueued = false;
        }
    }

    void resetHp()
    {
        Hp = FullHp;
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
            Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * groundRayLength);
        }
    }
}
