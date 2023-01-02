using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    [SerializeField] private GameObject plantBullet;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private float shootingRange;

    [SerializeField] private float attackCoolDownTime;
    private float attackCoolDownTimeCounter;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        attackCoolDownTimeCounter = attackCoolDownTime;
    }
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if(distance < shootingRange)
        {
            attackCoolDownTimeCounter -= Time.deltaTime;

            if (attackCoolDownTimeCounter < 0 && SystemVariable.gameController._state == stateOfGame.Play.ToString())
            {
                anim.SetTrigger("attack");
                attackCoolDownTimeCounter = attackCoolDownTime;
            }
        }

       if(SystemVariable.gameController._state != stateOfGame.Play.ToString())
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

    void Shoot()
    {
        Instantiate(plantBullet, bulletPos.position, Quaternion.identity);
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
