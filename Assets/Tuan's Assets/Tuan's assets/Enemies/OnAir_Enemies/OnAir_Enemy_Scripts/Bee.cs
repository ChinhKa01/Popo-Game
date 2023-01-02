using UnityEngine;

public class Bee : Enemy
{
    [SerializeField] private Transform[] idlePoints;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float yOffSet;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float angrySpeed;

    private int idlePointIndex;
    private bool playerDetected;
    private Transform player;
    private float defaultSpeed;

    protected override void Awake()
    {
        base.Awake();
        defaultSpeed = speed;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        bool idle = idleTimeCounter > 0;
        anim.SetBool("idle", idle);

        idleTimeCounter -= Time.deltaTime;
        if (idle)
            return;

        playerDetected = Physics2D.OverlapCircle(playerCheck.position, checkRadius, whatIsPlayer);

        if (playerDetected && !isAngry)
        {
            isAngry = true;
            speed = angrySpeed;
        }

        if (!isAngry)
        {
            transform.position = Vector2.MoveTowards(transform.position, idlePoints[idlePointIndex].position, speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, idlePoints[idlePointIndex].position) < .1f)
            {
                idlePointIndex++;
                if (idlePointIndex >= idlePoints.Length)
                    idlePointIndex = 0;
            }
        }
        else
        {
            Vector2 newPosition = new Vector2(player.transform.position.x, player.transform.position.y + yOffSet);
            transform.position = Vector2.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

            float xDiff = transform.position.x - player.position.x;

            if (Mathf.Abs(xDiff) < .15f)
            {
                anim.SetTrigger("attack");
            }
        }
    }

    //Hàm xử lý tấn công player
    private void Attack()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);
        Debug.Log("Bee bullet has been instantiated");
        newBullet.GetComponent<Bee_Bullet>().SetupSpeed(0, -bulletSpeed);
        Destroy(newBullet, 3f);

        speed = defaultSpeed; 
        idleTimeCounter = idleTime;
        isAngry = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(playerCheck.position, checkRadius);
    }
}
