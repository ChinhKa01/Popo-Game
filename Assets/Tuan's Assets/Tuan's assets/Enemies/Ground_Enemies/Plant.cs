using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject player;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        CollisionCheck();
        idleTimeCounter -= Time.deltaTime;

        bool playerDetected = playerDetection.collider.GetComponent<PlayerController>() != null;
       

        if (idleTimeCounter < 0 && playerDetected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("attack");
        }

        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = Vector3.one;
            facingDirection = -1;
        }
        else if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingDirection = 1;
        }

    }

    //Hàm xử lý tấn công player
    private void Attack()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);

        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Dart.ToString()))
        {
            TakeDame(0.5);
        }

        if (collision.GetComponent<PlayerController>() != null)
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
