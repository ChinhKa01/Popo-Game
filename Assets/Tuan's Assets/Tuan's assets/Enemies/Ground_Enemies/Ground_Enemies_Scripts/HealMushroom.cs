using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMushroom : Enemy
{

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        CollisionCheck();
            WalkAround();
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


}
