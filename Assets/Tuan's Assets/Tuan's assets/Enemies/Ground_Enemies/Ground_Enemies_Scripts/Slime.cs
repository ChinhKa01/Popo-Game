using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private Transform minePoint;
    [SerializeField] private Transform minePrefabs;
    [SerializeField] private float dropTime;
    private float dropTimeCounter;
    protected override void Awake()
    {
        base.Awake();
    }
    void Update()
    {
        dropTimeCounter -= Time.deltaTime;
        if(dropTimeCounter <= 0)
        {
            Instantiate(minePrefabs, minePoint.position, minePoint.rotation);

            dropTimeCounter = dropTime;
        }
        WalkAround();
        CollisionCheck();
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
