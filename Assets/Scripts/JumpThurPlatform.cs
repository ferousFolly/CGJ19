using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThurPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 1;
    private float currentWaitTime;
    private bool isColliding;


    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        currentWaitTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (isColliding == true)
        {
            if (Input.GetButton("Down") && PlayerMovementScript.isGrounded)
            {
                effector.rotationalOffset = 180f;
                currentWaitTime = waitTime;
            }
        }

        currentWaitTime -= Time.deltaTime;

        if (currentWaitTime < 0) effector.rotationalOffset = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isColliding = false;
        }
    }
}
