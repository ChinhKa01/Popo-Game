using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [SerializeField] private Transform rayCast;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float rayCastLength;
    [SerializeField] private float attackDistance; //Khoảng cách ngắn nhất cho đòn tấn công
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackCoolDownTime; //Khoảng thời gian dừng giữa các đòn tấn công
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    private RaycastHit2D hit;
    private Transform target;
    private Animator anim;
    private float distance; //khoảng cách giữa chameleon với player
    private bool attackMode;
    private bool inRange; //Kiểm tra xem player vào tầm tấn công chưa
    private bool cooling; //Kiểm tra xem chameleon có nghỉ sau khi tấn công không
    private float attackCoolDownTimeCounter;

    void Awake()
    {
        SelectTarget();
        attackCoolDownTimeCounter = attackCoolDownTime;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if(!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Chameleon_Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, whatIsPlayer);
            RaycastDebugger();
        }

        //Khi player bị thấy
        if(hit.collider != null)
        {
            ChameleonLogic();
        }
        else if(hit.collider != null)
        {
            inRange = false;
        }

        if(inRange == false)
        {
            StopAttack();
        }
    }

    void ChameleonLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            CoolDown();
            anim.SetBool("attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canRun", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Chameleon_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        attackCoolDownTime = attackCoolDownTimeCounter; //Reset time khi player vào tầm tấn công
        attackMode = true; //kiểm tra xem chameleon có tấn công ko
        anim.SetBool("canRun", false);
        anim.SetBool("attack", true);
    }

    void CoolDown()
    {
        attackCoolDownTime -= Time.deltaTime;
        if(attackCoolDownTime <= 0 && cooling && attackMode)
        {
            cooling = false;
            attackCoolDownTime = attackCoolDownTimeCounter;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("attack", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.transform;
            inRange = true;
            Flip(); 
        }

        if (collision.gameObject.CompareTag(Tag.Dart.ToString()))
        {
            Enemy.instance.TakeDame(0.5);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDame();
        }
    }

    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if(attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);
        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)
        {
            rotation.y = 0f;
        }
        else
        {
            rotation.y = 180f;
        }

        transform.eulerAngles = rotation;
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
