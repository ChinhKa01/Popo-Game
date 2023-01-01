using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    [SerializeField] private GameObject plantBullet;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private float shootingRange;

    private float timer;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if(distance < shootingRange)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                anim.SetTrigger("attack");
            }
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
}
