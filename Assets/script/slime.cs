using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class slime : MonoBehaviour
{
    [Header("Target")]
    public Transform player;              // ลาก Player มา หรือปล่อยว่างให้หา Tag

    [Header("Chase Settings")]
    public float chaseRange = 6f;         // ระยะเริ่มไล่
    public float stopRange = 0.6f;        // ระยะหยุด (ไม่ชนติดตัว)
    public float moveSpeed = 2f;          // ความเร็ววิ่งไล่
    public float maxVerticalDiff = 1.2f;  // ต่างระดับแกน Y ได้เท่าไร (กันข้ามชั้น)

    [Header("Facing")]
    public bool facingRight = true;       // ทิศเริ่มต้นของสไปรท์ (ขวาคือ true)

    Rigidbody2D rb;
    bool isChasing;

    public int Hp;
    public bool Bigslime;

    public GameObject prefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // ตรวจระยะ (ใช้ 2D)
        float dist = Vector2.Distance(transform.position, player.position);
        float yDiff = Mathf.Abs(player.position.y - transform.position.y);

        // ไล่เฉพาะเมื่ออยู่ในระยะ และอยู่ระดับใกล้กัน (กันข้ามแพลตฟอร์ม)
        isChasing = (dist <= chaseRange) && (yDiff <= maxVerticalDiff);

        // พลิกหน้าให้หันทางผู้เล่น (เฉพาะตอนจะวิ่ง)
        if (isChasing)
        {
            float dirX = player.position.x - transform.position.x;
            if (dirX > 0f && !facingRight) Flip();
            else if (dirX < 0f && facingRight) Flip();
        }
        
        if (Hp <= 0)
        {
            if (Bigslime)
            {
                Instantiate(prefab, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 vel = rb.velocity;

        if (isChasing)
        {
            float dx = player.position.x - transform.position.x;

            // ถ้าใกล้มากพอ ให้หยุด (กันชนติด)
            if (Mathf.Abs(dx) <= stopRange)
            {
                vel.x = 0f;
            }
            else
            {
                float dir = Mathf.Sign(dx);            // -1 ซ้าย, 1 ขวา
                vel.x = dir * moveSpeed;               // เคลื่อนที่บนแกน X
            }
        }
        else
        {
            // นอกระยะ ไม่ไล่: หยุดแกน X (ปล่อยให้แรงตก/แรงโน้มถ่วงทำงานเฉพาะแกน Y)
            vel.x = 0f;
        }

        rb.velocity = vel;
    }

    void Flip()
    {
        facingRight = !facingRight;
        var s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 1f, 0.2f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
#endif
}
