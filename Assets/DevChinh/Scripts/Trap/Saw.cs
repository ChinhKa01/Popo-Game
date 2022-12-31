using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public Transform Pos1,Pos2;
    public float Speed;
    public bool isPingPong,isReverse;

    // Update is called once per frame
    void Update()
    {
        if (isPingPong)
        {
            if (isReverse)
            {
                transform.position = Vector3.Lerp(Pos2.position, Pos1.position, Mathf.PingPong(Time.time * Speed, 1.0f));
            }
            else
            {
                transform.position = Vector3.Lerp(Pos1.position, Pos2.position, Mathf.PingPong(Time.time * Speed, 1.0f));
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }
}
