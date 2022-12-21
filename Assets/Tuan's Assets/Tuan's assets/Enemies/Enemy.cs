using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDirection = -1;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatToIgnore;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected double HP;
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime;
                     protected float idleTimeCounter;

    protected RaycastHit2D playerDetection;
    protected RaycastHit2D dartDetection;
    protected bool wallDetected;
    protected bool groundDetected;
    protected bool isAngry;

    [HideInInspector] protected bool invincible;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (groundCheck == null)
            groundCheck = transform;
        if (wallCheck == null)
            wallCheck = transform;
    }
   
    //Hàm xử lý di chuyển xung quanh
    protected virtual void WalkAround()
    {
        if (idleTimeCounter <= 0)
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, 0);

        idleTimeCounter -= Time.deltaTime;


        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }

    //Hàm xử lý nhận sát thương
    public void TakeDame(double damage)
    {
        if (!invincible)
        {
            HP -= damage;
            BeAttacked();

            if (HP <= 0)
            {
                OnDestroy();
            }
        }
    }

    //Hàm xử lý nhận sắt thương khi bị player nhảy lên
    public virtual void DamageOnHead() => TakeDame(0.5);

    public void BeAttacked() => anim.SetTrigger("gotHit");

    public void OnDestroy() => Destroy(gameObject);


    //Hàm xử lý xoay hướng enemy
    protected virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    //Hàm xử lý kiểm tra va chạm
    protected virtual void CollisionCheck()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 25, ~whatToIgnore);
        dartDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 100, ~whatToIgnore);
    }

    protected virtual void OnDrawGizmos()
    {
        if(groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        if(wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));

        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));

    }
}
