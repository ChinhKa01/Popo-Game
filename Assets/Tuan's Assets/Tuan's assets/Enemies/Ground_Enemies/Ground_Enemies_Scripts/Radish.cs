using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : Enemy
{
    private RaycastHit2D groundBelowDetected;
    private bool groundAboveDetected;

    [SerializeField] private float ceillingDistance;
    [SerializeField] private float groundDistance;

    [SerializeField] private float angryTime;
    private float angryTimeCounter;

    [SerializeField] private float flyForce;
    
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        angryTimeCounter -= Time.deltaTime;

        if(angryTimeCounter < 0 && !groundAboveDetected)
        {
            rb.gravityScale = 1;    
            isAngry = false;
        }

        if (!isAngry)
        {
            if(groundBelowDetected && !groundAboveDetected)
            {
                rb.velocity = new Vector2(0, flyForce);
            }
        }
        else
        {
            if(groundBelowDetected.distance <= 1.25f)
                WalkAround();
        }
         
        CollisionCheck();
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("angry", isAngry);
        
    }

    public override void DamageOnHead()
    {
        if (!isAngry)
        {
            angryTimeCounter = angryTime;
            rb.gravityScale = 12;
            isAngry = true;
        }
        else if(isAngry)
        {
            base.DamageOnHead();
        }    
            
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        groundAboveDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingDistance, whatIsGround);
        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }

   
}
