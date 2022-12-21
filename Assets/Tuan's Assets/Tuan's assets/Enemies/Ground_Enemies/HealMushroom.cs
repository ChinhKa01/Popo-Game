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


}
