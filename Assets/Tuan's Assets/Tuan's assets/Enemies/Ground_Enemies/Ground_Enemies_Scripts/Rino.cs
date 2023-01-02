using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rino : Enemy
{
    [SerializeField]
    private float angrySpeed;
    [SerializeField]
    private float shockTime;
    private float shockTimeCounter;

    protected override void Awake()
    {
        base.Awake();
        invincible = true;
    }

    void Update()
    {
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 25, ~whatToIgnore);
        if (playerDetection.collider.GetComponent<PlayerController>() != null)
            isAngry = true;


        if (!isAngry)
        {
            if (idleTimeCounter <= 0)
                rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, 0);

            idleTimeCounter -= Time.deltaTime;   

            if (wallDetected || !groundDetected)
            {
                idleTimeCounter = idleTime;
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(angrySpeed * facingDirection, rb.velocity.y);
            if(wallDetected && invincible)
            {
                invincible = false;
                shockTimeCounter = shockTime;
            }

            if(shockTimeCounter <= 0 && !invincible)
            {
                invincible = true;
                Flip();
                isAngry = false;
            }
            shockTimeCounter -= Time.deltaTime;
        }
        CollisionCheck();
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

}


