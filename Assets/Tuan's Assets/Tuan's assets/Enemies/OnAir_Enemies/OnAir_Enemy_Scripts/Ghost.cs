using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] private float activeTime;
    private float activeTimeCounter;
    [SerializeField] private float[] xOffSet;

    private Transform player;
    private SpriteRenderer sr;
    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").transform;
        activeTimeCounter = activeTime;
        isAngry = true;
        invincible = true;
    }

    void Update()
    {
        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);


        if(activeTimeCounter < 0 && idleTimeCounter < 0 && isAngry)
        {
            anim.SetTrigger("disappear");
            isAngry = false;
            idleTimeCounter = idleTime;
            invincible = true;
        }

        if(activeTimeCounter < 0 && idleTimeCounter < 0 && !isAngry)
        {
            RandomAppearPosition();
            anim.SetTrigger("appear");
            isAngry = true;
            activeTimeCounter = activeTime;
            invincible = false;
        }

        if (facingDirection == -1 && transform.position.x < player.transform.position.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > player.transform.position.x)
            Flip();
    }

    //Hàm xử lý xuất hiện ngẫu nhiên
    private void RandomAppearPosition()
    {
        float _xOffSet = xOffSet[Random.Range(0, xOffSet.Length)];
        float _yOffSet = Random.Range(-10, 10);
        transform.position = new Vector2(player.transform.position.x + _xOffSet, player.transform.position.y + _yOffSet);
    }

    //Hàm xử lý biến mất
    public void Disappear() => sr.enabled = false;

    //Hàm xử lý xuất hiện
    public void Appear() => sr.enabled = true;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
