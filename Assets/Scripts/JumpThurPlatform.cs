using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThurPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 1;
    public float currentWaitTime;



    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        currentWaitTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentWaitTime -= Time.deltaTime;

        if (Input.GetButton("Down") && PlayerMovementScript.isGrounded)
        {

            effector.rotationalOffset = 180f;
            currentWaitTime = waitTime;
        }

        if (currentWaitTime < 0) effector.rotationalOffset = 0f;
    }
}
