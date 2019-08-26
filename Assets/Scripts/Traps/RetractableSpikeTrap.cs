using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractableSpikeTrap : MonoBehaviour
{
    private bool isActive;
    private bool trapActive;

    public float activeTime = 0.5f;
    public float delayTime = 0.1f;
    public float moveDistance = 1f;
    public float triggerDistance = 1f;
    /*
     0 = up
     1 = right
     2 = down
     3 = left
    */
    public int direction = 0;

    private Vector2 pos;
    private float currentActiveTime;
    private float curretnDelayTime;
    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curretnDelayTime = delayTime;
        currentActiveTime = activeTime;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.position, transform.position) < triggerDistance && isActive == false)
        {
            isActive = true;
        }

        if (isActive == true)
        {
            curretnDelayTime -= Time.deltaTime;
            if (curretnDelayTime < 0)
            {
                if (direction == 0) transform.position = new Vector2(pos.x, pos.y + moveDistance);
                else if (direction == 1) transform.position = new Vector2(pos.x + moveDistance, pos.y);
                else if (direction == 2) transform.position = new Vector2(pos.x, pos.y - moveDistance);
                else if (direction == 3) transform.position = new Vector2(pos.x - moveDistance, pos.y);


                trapActive = true;
                currentActiveTime -= Time.deltaTime;
                if (currentActiveTime < 0)
                {
                    transform.position = new Vector2(pos.x, pos.y);
                    isActive = false;
                    trapActive = false;
                    curretnDelayTime = delayTime;
                    currentActiveTime = activeTime;
                }
            }
        }
    }
}
