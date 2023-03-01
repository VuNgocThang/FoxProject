using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTest : Enemy
{
    [SerializeField] private float leftCap = 40;
    [SerializeField] private float rightCap = 60;
    [SerializeField] private float heightJump = 10f;
    [SerializeField] private float lengthJump = 3f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private bool facingLeft = true;
    public int availableJump;
    public int totalJumps = 3;
    [SerializeField] private bool isGrounded;
    private Collider2D coll;
    public enum State
    {
        jump,
        fall,
        idle
    }
    public State state = State.idle;

    private void Awake()
    {
        availableJump = totalJumps;
    }

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        setAnim();
    }
    private void FixedUpdate()
    {
        CheckGround();
    }

    private void MoveAndFlip()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                if (availableJump > 0)
                {
                    /* if(availableJump > 0)
                     {
                         forceJump(-1);
                         Debug.Log("jumped");
                         anim.SetBool("Jumping", true);
                     }*/


                    if (availableJump % 2 == 0)
                    {
                        jump(1);
                        anim.SetBool("Jumping", true);
                        transform.localScale = new Vector3(-1, 1);

                    }
                    else
                    {
                        jump(-1);
                        anim.SetBool("Jumping", true);
                        transform.localScale = new Vector3(1, 1);
                    }

                }


            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);

                }

                if (availableJump > 0)
                {
                    /* if (availableJump > 0)
                     {
                         forceJump(1);
                         Debug.Log("jumped");
                         anim.SetBool("Jumping", true);
                     }*/
                    if (availableJump % 2 == 0)
                    {
                        jump(-1);
                        anim.SetBool("Jumping", true);
                        transform.localScale = new Vector3(1, 1);

                    }
                    else
                    {
                        jump(1);
                        anim.SetBool("Jumping", true);
                        transform.localScale = new Vector3(-1, 1);

                    }
                }


            }
            else
            {
                facingLeft = true;
            }
        }
    }
    void jump(int hor)
    {
        availableJump--;
        //rb.AddForce(new Vector2(lengthJump* hor, heightJump), ForceMode2D.Impulse);
        rb.velocity = new Vector2(lengthJump * hor, heightJump);
        state = State.jump;
    }
    void setAnim()
    {
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                state = State.fall;
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        if (coll.IsTouchingLayers(ground))
        {
            state = State.idle;
            anim.SetBool("Falling", false);
            availableJump = totalJumps;

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

}
