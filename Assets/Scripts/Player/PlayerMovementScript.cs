﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody2D rb;

    // Horizontal movement
    public float speed;
    public float jumpForce;
    private float moveInput;
    private bool facingRight = true;

    // Jumping
    public static bool isGrounded;
    private bool landed;
    public Transform groundCheck;
    public float areaRadius;
    public LayerMask whatIsGround;

    // Extra smooth jump stuff
    public float fJumpPressRemeberTime;
    private float fJumpPressRemeber;
    public float fGroundedRemeberTime;
    private float fGroundedRemeber;
    public float fCutJumpHeight;

    // Fix collisions
    EdgeCollider2D groundCollider;
    BoxCollider2D jumpCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCollider = GetComponent<EdgeCollider2D>();
        jumpCollider = GetComponent<BoxCollider2D>();
        landed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // When player hits ground
        if (isGrounded)
        {
            //if (!landed) Instantiate(landParticles);
            landed = true;
            fGroundedRemeber = fGroundedRemeberTime;
        }
        else landed = false;

        // Jumping
        isGrounded = Physics2D.OverlapBox(groundCheck.position, transform.localScale * areaRadius, 0, whatIsGround);

        fJumpPressRemeber -= Time.deltaTime;
        fGroundedRemeber -= Time.deltaTime;


        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressRemeber = fJumpPressRemeberTime;
        }


        if (fJumpPressRemeber > 0 && fGroundedRemeber > 0)
        {
            fJumpPressRemeber = 0;
            fGroundedRemeber = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //Instantiate(jumpParticles);
        }

        // Fall when not jumping
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);

        // Chage collidiers
        if (rb.velocity.y > 0)
        {
            jumpCollider.enabled = true;
            groundCollider.enabled = false;

        }
        else
        {
            jumpCollider.enabled = false;
            groundCollider.enabled = true;
        }
        if (Input.GetButton("Down"))
        {
            jumpCollider.enabled = false;
            groundCollider.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, areaRadius, whatIsGround);

        
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}