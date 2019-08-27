using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {
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
    public LayerMask groundLayer;

    // Extra smooth jump stuff
    public float fCoyoteTime;
    public float fGroundedRememberTime;
    public float fCutJumpHeight;
    private float fLastGrounded;
    private float fLastJumpPress;

    // Fix collisions
    EdgeCollider2D groundCollider;
    BoxCollider2D jumpCollider;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        groundCollider = GetComponent<EdgeCollider2D>();
        jumpCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        // Horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // When player hits ground
        if (IsGrounded()) {
            fLastGrounded = Time.time; // store last time player was grounded
        }
        if (Input.GetButtonDown("Jump")) {
            fLastJumpPress = Time.time;
            if (fLastGrounded >= Time.time - fCoyoteTime)
                Jump();
        }
        if (Input.GetButtonUp("Jump") && !IsGrounded()) // shorted jump when jump button not held
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);

        // Change collidiers
        if (rb.velocity.y > 0) {
            jumpCollider.enabled = true;
            groundCollider.enabled = false;
        }
        else
        {
            jumpCollider.enabled = false;
            groundCollider.enabled = true;
        }
    }

    private void Jump() {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("jumped");
    }

    private void FixedUpdate() {
        rb.AddForce(new Vector2(moveInput * speed * 4, 0), ForceMode2D.Impulse);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y);
        if (moveInput == 0) {
            if (rb.velocity.x > 0)
                rb.velocity = new Vector2(rb.velocity.x - 1, rb.velocity.y);
            else if (rb.velocity.x < 0)
                rb.velocity = new Vector2(rb.velocity.x + 1, rb.velocity.y);
        }

        if (!facingRight && moveInput > 0)
            Flip();
        else if (facingRight && moveInput < 0)
            Flip();
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void JumpIfBuffered() {
        if (fLastJumpPress >= Time.time - 0.4f)
            Jump();
    }

    bool IsGrounded() {
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