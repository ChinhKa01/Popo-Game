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
