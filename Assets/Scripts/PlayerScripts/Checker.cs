using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public float previousVelocity;
    public bool falling;
    public bool jumping;
    float boundsLeft, boundsRight;
    float boundsUp, boundsDown;

    Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    void Start()
    {
        boundsLeft = -20;
        boundsRight = 20;
        boundsUp = 20;
        boundsDown = -10;
    }
    private void Update()
    {
        BoundsChecker();
    }
    private void FixedUpdate()
    {
        previousVelocity = player.rb.velocity.y;
        if (player.rb.velocity.y != 0)
        {
            if (player.rb.velocity.y > 0.1f)
            {
                Debug.Log("Jumping");
                falling = false;
                jumping = true;
            }
            else if (player.rb.velocity.y < -0.1f)
            {
                Debug.Log("Falling");
                falling = true;
                jumping = false;
            }
        }
        else
        {
            Debug.Log("Stopped");
            falling = false;
            jumping = false;
        }
    }
    public void BoundsChecker()
    {
        if(transform.position.x > boundsRight || transform.position.x < boundsLeft || transform.position.y > boundsUp || transform.position.y < boundsDown)
        {
            transform.position = new Vector3(0, 10, 0);
            player.lives--;
        }
    }

}
