using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Running,
        JumpUp,
        JumpFall,
        Idle,
        Hurt,
        Climb
    }
    public State state = State.Idle;
    public Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    public Animator anim;
    [SerializeField] private float jump = 15f;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float hurtForce = 10f;

    public int availableJump;
    public int totalJumps = 3;
    public bool isGrounded = false;

    //climb laddersTest
    [SerializeField] private float _vertical;
    public bool isClimbing;
    public float climbSpeed;

    private Vector3 respawnPoint;
    public GameObject fall;
    private List<GameObject> ladders = new List<GameObject>();

    private bool canMove = true;

    private void Awake()
    {
        availableJump = totalJumps;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        respawnPoint = transform.position;
    }


    void Update()
    {
        VelocityState();
       
        if (state != State.Hurt && canMove)
        {
            Movement();
        }
        fall.transform.position = new Vector2(transform.position.x, fall.transform.position.y);
    }

    private void CheckLadders()
    {
        if (ladders.Count > 0 && Mathf.Abs(_vertical) > 0f)
        {
            isClimbing = true;
            anim.Play("climb");
            state = State.Climb;

        }
        else if (ladders.Count <= 0)
        {
            isClimbing = false;
        }
    }
    void CheckGround()
    {
        if (coll.IsTouchingLayers(ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void FixedUpdate()
    {
        CheckGround();
        CheckLadders();

        if (isClimbing)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 4f;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
        }
        if (collision.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            jump = 20f;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(ResetPower());

        }
        if (collision.CompareTag("Ladders"))
        {
            UnityEngine.Debug.Log("isClimbing is True" + collision.gameObject);
            ladders.Add(collision.gameObject);
            isClimbing = true;
        }
        if (collision.gameObject.tag == "Fall")
        {
            HeartsHandle();
            transform.position = respawnPoint;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladders"))
        {
            UnityEngine.Debug.Log("isClimbing is False");

            ladders.Remove(collision.gameObject);
            isClimbing = false;
            state = State.Idle;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.JumpFall)
            {
                enemy.JumpedOn();
                JumpOrClimb();
            }
            else
            {
                anim.Play("hurt");
                state = State.Hurt;
                canMove = false;
                availableJump = totalJumps;
                StartCoroutine(TimeHurt());
                HeartsHandle();
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }

        }

    }

    public void HeartsHandle()
    {
        PermanentUI.perm.hearts -= 1;
        PermanentUI.perm.heartsText.text = PermanentUI.perm.hearts.ToString();
        if (PermanentUI.perm.hearts <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerPrefs.DeleteAll();
        }
    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (availableJump > 0 && !isClimbing)
            {
                JumpOrClimb();
                anim.Play("jump_up");
            }
        }
        if (availableJump > 0 && !isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                JumpOrClimb();
                anim.Play("jump_up");
            }
        }
        else if (isClimbing)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                JumpOrClimb();
                anim.Play("climb");
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (isClimbing)
            {
                ClimbDown();
                anim.Play("climb");
            }
        }
    }


    public void JumpOrClimb()
    {
        _vertical = 1;
        if (availableJump > 0 && !isClimbing)
        {
            UnityEngine.Debug.Log("Jump Jump");

            anim.Play("jump_up");
            rb.velocity = new Vector2(rb.velocity.x, jump);
            state = State.JumpUp;
            availableJump--;
        }
        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, _vertical * climbSpeed);
            anim.Play("climb");
        }

    }
    public void ClimbDown()
    {
        _vertical = -1;
        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, _vertical * climbSpeed);
            anim.Play("climb");
        }
    }

    public void MoveLeft()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
        transform.localScale = new Vector2(-1, 1);
    }
    public void MoveRight()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        transform.localScale = new Vector2(1, 1);
    }

    private void VelocityState()
    {
        if (state == State.Climb || isClimbing)
        {
            //Climb();
            anim.Play("climb");
        }
        else if (state == State.JumpUp)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.JumpFall;
                anim.Play("jump_fall");
            }
        }
        else if (state == State.JumpFall)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.Idle;
                anim.Play("idle");
                availableJump = totalJumps;

            }
        }
        else if (state == State.Hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.Idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.Running;
            anim.Play("run");
        }
        else
        {
            state = State.Idle;
            anim.Play("idle");
        }
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jump = 15;
        GetComponent<SpriteRenderer>().color = Color.white;

    }
    private IEnumerator TimeHurt()
    {
        yield return new WaitForSeconds(1);
        canMove = true;
    }
}
