using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Bullet : MonoBehaviour
{
    [SerializeField] private float force;

    private Rigidbody2D rb;
    private GameObject player;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 distance = player.transform.position - transform.position;
        rb.velocity = new Vector2(distance.x, distance.y).normalized * force;

        float rot = Mathf.Atan2(-distance.y, -distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnBecameInvisible()
    {
        Destroy(gameObject);
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
