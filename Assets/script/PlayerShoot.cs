using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;   // พรีแฟ็บกระสุน
    public Transform shootPoint;      // จุดปล่อย (ปลายไม้เท้า)
    public float shootCooldown = 0.5f;
    private float cooldownTimer;

    public bool SaveZone=false;

    private bool isFacingRight = true; // ใช้ตรวจทิศที่หัน (ตาม Player)

    public GameObject Player;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0 && !SaveZone)
        {
            Shoot();
            cooldownTimer = shootCooldown;
            Player.GetComponent<Player>().Hp -= 10;
        }

        // อัปเดตทิศหันตามการเคลื่อนที่
        float move = Input.GetAxisRaw("Horizontal");
        if (move > 0) isFacingRight = true;
        else if (move < 0) isFacingRight = false;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // ยิงตามทิศหัน
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        bullet.GetComponent<magic>().SetDirection(direction);
    }
}
