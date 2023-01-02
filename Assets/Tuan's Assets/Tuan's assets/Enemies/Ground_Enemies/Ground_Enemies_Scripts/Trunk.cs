using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    [SerializeField] private GameObject trunkBullet;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private float shootingRange;
    private GameObject player;

    [SerializeField] private float attackCoolDownTime;
    private float attackCoolDownTimeCounter;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(trunkBullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        attackCoolDownTimeCounter = attackCoolDownTime;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < shootingRange)
        {
            attackCoolDownTimeCounter -= Time.deltaTime;

            if (attackCoolDownTimeCounter < 0 && SystemVariable.gameController._state == stateOfGame.Play.ToString())
            {
                anim.SetTrigger("attack");
                attackCoolDownTimeCounter = attackCoolDownTime;
            }
        }

        if (SystemVariable.gameController._state != stateOfGame.Play.ToString())
        {
            attackCoolDownTimeCounter += Time.deltaTime;
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
        Instantiate(trunkBullet, bulletPos.position, Quaternion.identity);
    }


    protected override void CollisionCheck()
    {
        base.CollisionCheck();


    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
    