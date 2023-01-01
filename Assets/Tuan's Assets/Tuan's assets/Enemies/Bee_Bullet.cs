using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    private float xSpeed;
    private float ySpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    //Hàm xử lý tốc độ bay của đạn
    public void SetupSpeed(float x, float y)
    {
        xSpeed = x;
        ySpeed = y;
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
        Destroy(gameObject, 1);
    }

}
