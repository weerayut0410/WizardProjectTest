using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic : MonoBehaviour
{
    public float speed = 10f;     // ��������
    public float lifeTime = 2f;   // ���������Թҷա�͹���
    public int damage ;     // �����

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            damage = Random.Range(3,6);
            collision.GetComponent<slime>().Hp -= damage;
            Destroy(gameObject);
        }

    }
}
