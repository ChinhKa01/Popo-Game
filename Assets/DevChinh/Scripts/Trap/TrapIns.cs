using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapIns : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDame();
            Destroy(gameObject);
        }
    }
}
