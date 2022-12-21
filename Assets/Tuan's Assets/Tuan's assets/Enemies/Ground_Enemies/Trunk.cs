using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform groundBehindCheck;

    [SerializeField] private float attackCoolDown;
    private float attackCoolDownCounter;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject player;

    [SerializeField] private float moveBackTime;
    private float moveBackTimeCounter;

    private bool playerDetected;
    private bool wallBehind;
    private bool groundBehind;
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        CollisionCheck();
        anim.SetFloat("xVelocity", rb.velocity.x);


        attackCoolDownCounter -= Time.deltaTime;
        moveBackTimeCounter -= Time.deltaTime;

        if (playerDetected)
            moveBackTimeCounter = moveBackTime;

        if(playerDetection.collider.GetComponent<PlayerController>() != null)
        {
            if (attackCoolDownCounter < 0)
            {
                attackCoolDownCounter = attackCoolDown;
                anim.SetTrigger("attack");
            }
            else if (playerDetection.distance < 3)
                MoveBackWard(1.5f);
        }
        else
        {
            if (moveBackTimeCounter > 0)
                MoveBackWard(4);
            else
                WalkAround();
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
        Destroy(newBullet, 3f);
    }

    //Hàm xử lý lùi về khi phát hiện player
    private void MoveBackWard(float multi)
    {
        if (wallBehind)
            return;

        if (!groundBehind)
            return;

        rb.velocity = new Vector2(speed * multi * -facingDirection, rb.velocity.y);
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


    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        groundBehind = Physics2D.Raycast(groundBehindCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallBehind = Physics2D.Raycast(wallCheck.position, Vector2.right * (-facingDirection + 1), wallCheckDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.DrawLine(groundBehindCheck.position, new Vector2(groundBehindCheck.position.x, groundBehindCheck.position.y - groundCheckDistance));
    }
}
