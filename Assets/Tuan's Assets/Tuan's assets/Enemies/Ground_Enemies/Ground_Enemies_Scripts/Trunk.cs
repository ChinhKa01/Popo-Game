using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    [SerializeField] private GameObject trunkBullet;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private float shootingRange;
    private GameObject player;
    private float timer;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(trunkBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < shootingRange)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                anim.SetTrigger("attack");
            }

            if (transform.position.x < player.transform.position.x)
            {
                transform.localScale = Vector3.one;
                facingDirection = -1;
            }
            else if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingDirection = 1;
            }
        }
        else if(distance > shootingRange)
        {
            WalkAround();
        }


       

        CollisionCheck();
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    //Hàm xử lý tấn công player
    private void Attack()
    {
        Instantiate(trunkBullet, bulletPos.position, Quaternion.identity);
    }


    protected override void CollisionCheck()
    {
        base.CollisionCheck();

       
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Dart.ToString()))
        {
            TakeDame(0.5);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }


}
