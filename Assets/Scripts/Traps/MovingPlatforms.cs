using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public float speed;
    [SerializeField] private Direction direction = Direction.left;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case Direction.up:
                transform.Translate(0, speed * Time.deltaTime, 0);
                break;
            case Direction.right:
                transform.Translate(speed * Time.deltaTime, 0, 0);
                break;
            case Direction.down:
                transform.Translate(0, -speed * Time.deltaTime, 0);
                break;
            case Direction.left:
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "Player")
        {
            switch (direction)
            {
                case Direction.up:
                    direction = Direction.down;
                    break;
                case Direction.right:
                    direction = Direction.left;
                    break;
                case Direction.down:
                    direction = Direction.up;
                    break;
                case Direction.left:
                    direction = Direction.right;
                    break;
                default:
                    break;
            }
            print("Direction Changed");
        }
        print("Hit");
    }
}
