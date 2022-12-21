using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public Vector3 Pos1,Pos2;
    public float Speed;
    public bool isPingPong;

    // Update is called once per frame
    void Update()
    {
        if (isPingPong)
        {
            transform.position = Vector3.Lerp(Pos1, Pos2, Mathf.PingPong(Time.time * Speed, 1.0f));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }
}
