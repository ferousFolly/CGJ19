using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScriptOld : MonoBehaviour {
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
    public LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private bool debugGroundCollision;

    // Extra smooth jump stuff
    public float fJumpPressRememberTime;
    private float fJumpPressRemember;
    public float fGroundedRememberTime;
    private float fGroundedRemember;
    public float fCutJumpHeight;

    // Fix collisions
    EdgeCollider2D groundCollider;
    BoxCollider2D jumpCollider;

    // Platform
    private bool jumpThurCollision;

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
        if (IsGrounded())
            fGroundedRemember = fGroundedRememberTime;

        // Jumping
        fJumpPressRemember -= Time.deltaTime;
        fGroundedRemember -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) {
            fJumpPressRemember = fJumpPressRememberTime;
        }

        if (fJumpPressRemember > 0 && fGroundedRemember > 0) {
            fJumpPressRemember = 0;
            fGroundedRemember = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //Instantiate(jumpParticles);
        }

        // Fall when not jumping
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);

        // Chage collidiers
        if (jumpThurCollision == true) {
            jumpCollider.enabled = true;
            groundCollider.enabled = false;
        }
        else {
            jumpCollider.enabled = false;
            groundCollider.enabled = true;
        }
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (!facingRight && moveInput > 0) {
            Flip();
        }
        else if (facingRight && moveInput < 0) {
            Flip();
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "JumpThurPlatform") {
            jumpThurCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "JumpThurPlatform") {
            jumpThurCollision = false;
        }
    }

    bool IsGrounded() {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = groundCheckDistance;

        if (debugGroundCollision)
            Debug.DrawRay(position, direction, Color.green); // show groundchecker ray

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (hit.collider != null)
            return true; // grounded
        return false; // not grounded
    }

}