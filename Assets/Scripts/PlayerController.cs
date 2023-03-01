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

    private List<GameObject> ladders = new List<GameObject>();

    private void Awake()
    {
        availableJump = totalJumps;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        // PermanentUI.perm.heartsText.text = PermanentUI.perm.hearts.ToString();
    }


    void Update()
    {
        _vertical = Input.GetAxis("Vertical");

        VelocityState();
        if (state != State.Hurt)
        {
            Movement();
        }
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

    private void FixedUpdate()
    {
        CheckGround();
        CheckLadders();

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            JumpOrClimb();
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
            jump = 15f;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(ResetPower());

        }
        if (collision.CompareTag("Ladders"))
        {
            UnityEngine.Debug.Log("isClimbing is True");
            ladders.Add(collision.gameObject);
            isClimbing = true;  
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
                availableJump = totalJumps;

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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (availableJump > 0 && !isClimbing)
            {
                //JumpUp();
                JumpOrClimb();
                anim.Play("jump_up");
            }
            if (isClimbing)
            {
                //Climb();
                JumpOrClimb();
                anim.Play("climb");
            }
        }
    }
   /* public void JumpUp()
    {

        anim.Play("jump_up");
        rb.velocity = new Vector2(rb.velocity.x, jump);
        state = State.JumpUp;
        availableJump--;

    }

    public void Climb()
    {
        rb.velocity = new Vector2(rb.velocity.x, _vertical * climbSpeed);
    }*/

    public void JumpOrClimb()
    {
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
           // UnityEngine.Debug.Log("Climb Climb");

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
        if (state == State.Climb || isClimbing )
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
        else if (rb.velocity.x != 0 && rb.velocity.y <= 0.1f )
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
        jump = 10f;
        GetComponent<SpriteRenderer>().color = Color.white;

    }


    void CheckGround()
    {
        if (coll.IsTouchingLayers(ground))
        {
            isGrounded = true;
            availableJump = totalJumps;
        }
        else
        {
            isGrounded = false;
        }
    }


}
