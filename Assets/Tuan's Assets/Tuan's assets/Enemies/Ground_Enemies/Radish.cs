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
            invincible = true;
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
            invincible = false;
        }
        else if(isAngry)
        {
            base.DamageOnHead();
        }    
            
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
