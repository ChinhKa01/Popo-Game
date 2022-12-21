using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : Trap
{
    public bool isWorking;
    private Animator anim;
    public float repeatRate;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        InvokeRepeating("FireSwitch", 0, repeatRate);
    }

    private void Update()
    {
        anim.SetBool("isWorking", isWorking);
    }

    public void FireSwitch()
    {
        isWorking = !isWorking;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(isWorking)
            base.OnTriggerEnter2D(collision);
    }
}
