using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[System.Serializable]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _speed, _forceJump, _forceShoot,_speedOfCoint;
    [SerializeField] private float _radiusOfGroundCheckPoint, _TimeToAttack, _TimeToTelePos, _radiusOfAttackPoint,_radiusTakeCoint;
    [SerializeField] private Transform groundCheckPoint, attackPoint;
    [SerializeField] private GameObject jumpEffect, teleportEffect, runEffect;
    private bool _isGround, _isJumping, _isTelePortting, _isPlatform;
    private float _xDir, _yDir, CountDown, CountDown2;
    private Rigidbody2D rid;
    public Animator animator;
    private float _scaleX, _scaleY;
    public GameObject[] listHeart;
    public int _damage,dameOff, poolingAmount,_damageRand;
    public LayerMask Ground, Enemy, Platform, Button,Coint;
    public GameObject damagePopups;
    private Queue<GameObject> stack;
    public GameObject dartObj, dartPool;
    public List<GameObject> listIns;
    public Vector2 minCam, maxCam;
    public AudioClip attackSound, attack2Sound;
    public float forceUp;
    public Slider sliderTime, sliderTime2;
    public Text textTimeAttack, textTimeTele;
    private void Awake()
    {
        SystemVariable.playerController = this;
        animator = GetComponent<Animator>();
        rid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Prefs.SPEED == 0)
        {
            Prefs.SPEED = _speed;
        }
        if (Prefs.DAME == 0)
        {
            Prefs.DAME = _damage;
        }
        if (Prefs.TIMEATTACK == 0)
        {
            Prefs.TIMEATTACK = _TimeToAttack;
        }
        if (Prefs.TIMETELE == 0)
        {
            Prefs.TIMETELE = _TimeToTelePos;
        }
        sliderTime2.maxValue = _TimeToTelePos;
        sliderTime2.value = 0;
        sliderTime.maxValue = _TimeToAttack;
        sliderTime.value = 0;
        CountDown = CountDown2 = 0;
        _isPlatform = false;
        _isJumping = false;
        _isTelePortting = false;
        _scaleX = transform.localScale.x;
        _scaleY = transform.localScale.y;
        animator.SetTrigger(stateOfPlayer.Appear.ToString());
        runEffect.SetActive(false);
        SystemVariable.quantityCurrentHeart = listHeart.Length;
        stack = new Queue<GameObject>();
       
        /* for (int i = 0; i < poolingAmount; i++)
         {
             GameObject obj = Instantiate(dartObj, transform.position, Quaternion.identity);
             obj.SetActive(false);
             obj.transform.parent = dartPool.transform;
             listIns.Add(obj);
         }*/
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("pref:" + Prefs.LIFE + "Life:" + SystemVariable.quantityCurrentHeart + "State:" + SystemVariable.gameController._state);
        _xDir = Input.GetAxisRaw("Horizontal");
        autoTakeCoint();
        if (SystemVariable.gameController._state != stateOfGame.GameOver.ToString())
        {
            ButtonEventHandle();
            CheckCircle();
            Rotate();
            SpecialAnimationHandling();
            Attack();
        }
        else
        {
            animator.SetTrigger(stateOfPlayer.Death.ToString());
        }
        updateSliderAttack();
        updateSliderTele();
    }

    private void FixedUpdate()
    {     
        if (SystemVariable.gameController._state != stateOfGame.GameOver.ToString())
        {
            Movement();
            Jumping();
            Teleportting();
        }
    }

    public void autoTakeCoint()
    {
        if (SystemVariable.gameController.isAutoTakeCoint)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position,_radiusTakeCoint,Coint);
            foreach(Collider2D obj in col)
            {
                obj.gameObject.transform.position = Vector2.MoveTowards(obj.gameObject.transform.position, transform.position, _speedOfCoint * Time.deltaTime);
            }
        }
    }

    //Hàm xử lý sự kiện nhập từ bàn phím
    private void ButtonEventHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _isJumping = true;
        }

        if (CountDown2 <= 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _isTelePortting = true;
                CountDown2 = Prefs.TIMETELE;
            }
        }
        else
        {
            CountDown2 -= Time.deltaTime;
        }
    }

    //Hàm xử lý hành động di chuyển player trên trục x
    private void Movement()
    {
        rid.velocity = new Vector2(_xDir * _speed, rid.velocity.y);

        minCam = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxCam = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCam.x, maxCam.x),
                                          /*Mathf.Clamp(transform.position.y, minCam.y, maxCam.y)*/transform.position.y,
                                          0);

        if (_xDir != 0 && _isGround)
        {
            runEffect.SetActive(true);
        }
        else
        {
            runEffect.SetActive(false);
        }
    }

    //Hàm xử lý hành động nhảy của Player
    private void Jumping()
    {
        if (_isJumping && _isGround)
        {
            Instantiate(jumpEffect, transform.position, Quaternion.identity);
            rid.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
            _isJumping = false;
        }
    }

    //Hàm xử lý hoạt ảnh nhảy lên và lúc rơi xuống của Player
    private void SpecialAnimationHandling()
    {
        //Movement
        animator.SetFloat(stateOfPlayer.Run.ToString(), Mathf.Abs(_xDir));

        //Jumping
        if (_isGround == false)
        {
            if (rid.velocity.y > 0.01f)
            {
                animator.SetBool(stateOfPlayer.IsJumpingUp.ToString(), true);
            }
            else
            {
                animator.SetBool(stateOfPlayer.IsJumpingUp.ToString(), false);
                animator.SetBool(stateOfPlayer.IsJumpingDown.ToString(), true);
            }
        }
        else
        {
            animator.SetBool(stateOfPlayer.IsJumpingUp.ToString(), false);
            animator.SetBool(stateOfPlayer.IsJumpingDown.ToString(), false);
        }
    }

    //Hàm xử lý hành động xoay Player bằng phương pháp Scale
    private void Rotate()
    {
        if (_xDir < 0)
        {
            transform.localScale = new Vector2(-_scaleX, _scaleY);
            attackPoint.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (_xDir > 0)
        {
            transform.localScale = new Vector2(_scaleX, _scaleY);
            attackPoint.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    public void updateSliderAttack()
    {
        if (CountDown <= 0)
        {
            textTimeAttack.text = "";
            sliderTime.value = 0;
            return;
        }
        textTimeAttack.text = string.Format("{0:f2}", CountDown) + "s";
        sliderTime.value = CountDown;
    }


    public void updateSliderTele()
    {
        if (CountDown2 <= 0)
        {
            textTimeTele.text = "";
            sliderTime2.value = 0;
            return;
        }
        textTimeTele.text = string.Format("{0:f2}", CountDown2) + "s";
        sliderTime2.value = CountDown2;
    }

    //Hàm xử lý hành động dịch chuyển tức thời của Player
    private void Teleportting()
    {
        if (_isTelePortting)
        {
            animator.SetTrigger(stateOfPlayer.Teleport.ToString());
            GameObject obj = Instantiate(teleportEffect, transform.position, Quaternion.identity);
            obj.transform.localScale = transform.localScale;
            rid.AddForce(Vector2.right * _forceJump * transform.localScale.x, ForceMode2D.Impulse);
            _isTelePortting = false;
        }
    }

    //Hàm xử lý hành động tấn công của Player
    private void Attack()
    {
        if (CountDown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.position, _radiusOfAttackPoint, Enemy);
                Collider2D Door = Physics2D.OverlapCircle(attackPoint.position, _radiusOfAttackPoint, Button);
                RandomDamage();
                if (Door != null)
                {
                    SystemVariable.gameController.Door.GetComponent<Animator>().SetBool("OPen", true);
                    SystemVariable.audioController.Play(attackSound);
                    Door.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    animator.SetTrigger(stateOfPlayer.Attack.ToString());
                }
                else if (enemy.Length > 0)
                {
                    SystemVariable.audioController.Play(attackSound);
                    foreach (var obj in enemy)
                    {
                        Instantiate(damagePopups, obj.transform.position, Quaternion.identity);
                        obj.GetComponent<BossController>().TakeDame(_damageRand);
                    }
                    animator.SetTrigger(stateOfPlayer.Attack.ToString());
                }
                else
                {
                    SystemVariable.audioController.Play(attack2Sound);
                    animator.SetTrigger(stateOfPlayer.Attack2.ToString());
                    //GameObject obj = Instantiate(dartObj, attackPoint.position, Quaternion.identity);
                    //obj.GetComponent<Rigidbody2D>().AddForce(attackPoint.right * _forceShoot);
                    /* if (stack.Count == 0)
                     {
                         foreach (GameObject ob in listIns)
                         {
                             stack.Enqueue(ob);
                         }
                     }
                     GameObject dart = stack.Dequeue();
                     dart.transform.position = attackPoint.position;
                     dart.SetActive(true);
                     dart.GetComponent<Rigidbody2D>().AddForce(attackPoint.right * _forceShoot);*/
                    Instantiate(dartObj, attackPoint.position, attackPoint.rotation);
                }
                CountDown = Prefs.TIMEATTACK;
            }
        }
        else
        {
            CountDown -= Time.deltaTime;
        }
    }

    public void TakeDame()
    {
        try
        {
            animator.SetTrigger(stateOfPlayer.Teleport.ToString());
            listHeart[SystemVariable.quantityCurrentHeart - 1].SetActive(false);
            SystemVariable.quantityCurrentHeart--;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Lỗi" + ex);
        }
    }

    //Hàm kiểm tra xem Player có ở trên mặt đất hay không?
    private void CheckCircle()
    {
        _isGround = Physics2D.OverlapCircle(groundCheckPoint.position, _radiusOfGroundCheckPoint, Ground);
        _isPlatform = Physics2D.OverlapCircle(groundCheckPoint.position, _radiusOfGroundCheckPoint, Platform);
        if (_isPlatform)
        {
            _isGround = true;
        }
    }

    public void Disable() => this.enabled = false;

    public void ActiveDialogWithEvent() => SystemVariable.gameController.OpenDialog();

    public void RandomDamage()
    {
        _damageRand = Random.Range(Prefs.DAME - dameOff, Prefs.DAME + dameOff);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Rubi.ToString()))
        {
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("addForce"))
        {
            rid.AddForce(new Vector2(0, forceUp));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Port"))
        {
            transform.position = collision.gameObject.transform.position;
            animator.SetTrigger(stateOfPlayer.Win.ToString());
            this.enabled = false;
        }

        if (collision.gameObject.CompareTag("ArrowZone"))
        {
            SystemVariable.gameController.Arrow.SetActive(false);
        }

        if (collision.gameObject.CompareTag("DeathZone"))
        {
            SystemVariable.gameController._state = stateOfGame.GameOver.ToString();
        }
    }

    public void Win()
    {
        SystemVariable.gameController._state = stateOfGame.Win.ToString();
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, _radiusOfGroundCheckPoint);
        Gizmos.DrawWireSphere(attackPoint.position, _radiusOfAttackPoint);
        Gizmos.DrawWireSphere(transform.position, _radiusTakeCoint);
    }
}

