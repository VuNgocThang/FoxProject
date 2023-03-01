using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : Enemy
{
    [SerializeField] private float maxHeightJump = 6;
    [SerializeField] private float forceJump = 6f;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private bool isGrounded = false;
    float z = 180;
    

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        if (transform.position.y > maxHeightJump)
        {
            Rotate();

        }
        setAnim();
    }

    private void FixedUpdate()
    {
        if (coll.IsTouchingLayers(ground))
        {
            Jump();
            isGrounded= true;   
        }
        else
        {
            isGrounded= false;  
        }
    }

    void setAnim()
    {

        if (rb.velocity.y > 0.1f)
        {
            anim.Play("dino_jump");
        }
        if (transform.position.y > maxHeightJump)
        {
            anim.Play("rotate");
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * forceJump , ForceMode2D.Impulse);
    }
    void Rotate()
    {
        rb.velocity = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(0, 0, z);
        transform.rotation = rotation;
    
    }
}
