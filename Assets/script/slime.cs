using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class slime : MonoBehaviour
{
    [Header("Target")]
    public Transform player;              // �ҡ Player �� ���ͻ������ҧ����� Tag

    [Header("Chase Settings")]
    public float chaseRange = 6f;         // ������������
    public float stopRange = 0.6f;        // ������ش (��誹�Դ���)
    public float moveSpeed = 2f;          // ��������������
    public float maxVerticalDiff = 1.2f;  // ��ҧ�дѺ᡹ Y ������� (�ѹ�������)

    [Header("Facing")]
    public bool facingRight = true;       // ���������鹢ͧ��÷� (��Ҥ�� true)

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

        // ��Ǩ���� (�� 2D)
        float dist = Vector2.Distance(transform.position, player.position);
        float yDiff = Mathf.Abs(player.position.y - transform.position.y);

        // ���੾���������������� ��������дѺ���ѹ (�ѹ�����ŵ�����)
        isChasing = (dist <= chaseRange) && (yDiff <= maxVerticalDiff);

        // ��ԡ˹������ѹ�ҧ������ (੾�е͹�����)
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

            // �������ҡ�� �����ش (�ѹ���Դ)
            if (Mathf.Abs(dx) <= stopRange)
            {
                vel.x = 0f;
            }
            else
            {
                float dir = Mathf.Sign(dx);            // -1 ����, 1 ���
                vel.x = dir * moveSpeed;               // ����͹��躹᡹ X
            }
        }
        else
        {
            // �͡���� ������: ��ش᡹ X (���������ç��/�ç�����ǧ�ӧҹ੾��᡹ Y)
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
