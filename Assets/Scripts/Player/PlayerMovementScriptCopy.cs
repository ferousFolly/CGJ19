using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScriptCopy : MonoBehaviour
{
    private Rigidbody2D rb;

    // Horizontal movement
    public float speed;
    public float jumpForce;
    private float moveInput;
    private bool facingRight = true;

    // Jumping
    public static bool isGrounded;
    public Transform groundChecker;
    public float groundCollisionDistance;
    public bool debugGroundCollision;
    public float areaRadius;
    public LayerMask groundLayer;

    // Extra smooth jump stuff
    public float fCoyoteTime;
    private float fLastJumpPress;
    public float fGroundedRememberTime;
    private float fLastGrounded;
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
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // When player hits ground
        if (IsGrounded())
            fLastGrounded = Time.time;
        if (Input.GetButtonDown("Jump") && fLastGrounded >= Time.time - fCoyoteTime)
            Jump();
        /* I don't really understand what eny of this does besides jump
        The above if statements manage quite well, so I'm leaving this commented out for now

       // Jumping
       fLastJumpPress -= Time.deltaTime;
       fLastGrounded -= Time.deltaTime;

       if (Input.GetButtonDown("Jump"))
          fLastJumpPress = fCoyoteTime;

       if (fLastJumpPress > 0 && fLastGrounded > 0)
       {
           fLastJumpPress = 0;
           fLastGrounded = 0;
           rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           //Instantiate(jumpParticles);
       }

       //Fall when not jumping
       if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);
       */

        // Change collidiers
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

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("jumped");
    }

    private void FixedUpdate()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, areaRadius, whatIsGround);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (!facingRight && moveInput > 0)
            Flip();
        else if (facingRight && moveInput < 0)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = groundCollisionDistance;

        if (debugGroundCollision)
            Debug.DrawRay(position, direction, Color.green); // show groundchecker ray

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (hit.collider != null)
            return true; // grounded

        return false; // not grounded
    }
}
