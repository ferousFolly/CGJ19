using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction { up, right, down, left }

public class RetractableSpikeTrap : MonoBehaviour
{
    private bool isActive;
    private bool trapActive;

    public float activeTime = 0.5f;
    public float delayTime = 0.1f;
    public float moveDistance = 1f;
    public float triggerDistance = 1f;

    [SerializeField] private Direction direction = Direction.up;

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
            isActive = true;

        if (isActive == true)
        {
            curretnDelayTime -= Time.deltaTime;
            if (curretnDelayTime < 0)
            {
                switch (direction)
                {
                    case Direction.up:
                        transform.position = new Vector2(pos.x, transform.position.y + ((pos.y + moveDistance) - transform.position.y) * 0.5f);
                        break;
                    case Direction.right:
                        transform.position = new Vector2(transform.position.x + ((pos.x + moveDistance) - transform.position.x) * 0.5f, pos.y);
                        break;
                    case Direction.down:
                        transform.position = new Vector2(pos.x, transform.position.y + ((pos.y - moveDistance) - transform.position.y) * 0.5f);
                        break;
                    case Direction.left:
                        transform.position = new Vector2(transform.position.x + ((pos.x - moveDistance) - transform.position.x) * 0.5f, pos.y);
                        break;
                    default:
                        break;
                }

                trapActive = true;
                currentActiveTime -= Time.deltaTime;

                if (Vector2.Distance(player.position, transform.position) > triggerDistance)
                {
                    if (currentActiveTime < 0)
                    {
                        transform.position = new Vector2(transform.position.x + (pos.x - transform.position.x) * 0.5f, transform.position.y + (pos.y - transform.position.y) * 0.5f);
                        isActive = false;
                        trapActive = false;
                        curretnDelayTime = delayTime;
                        currentActiveTime = activeTime;
                    }
                }
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x + (pos.x - transform.position.x) * 0.5f, transform.position.y + (pos.y - transform.position.y) * 0.5f);
        }
    }
}
