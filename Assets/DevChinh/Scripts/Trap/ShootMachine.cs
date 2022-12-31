using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMachine : MonoBehaviour
{
    public GameObject spike;

    public float timeToShoot,force;
    private float count;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(count <= 0)
        {
            GameObject obj = Instantiate(spike, transform.position, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,force));
            count = timeToShoot;
        }
        else
        {
            count -= Time.deltaTime;
        }
    }
}
