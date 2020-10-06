using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public float previousVelocity;
    private Vector3 rayCastOrigin;
    public Vector3 rayCastOffset;

    private float groundCheckRayLength = 0.2f;

    private bool falling;
    public float distanceToGround;

    Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    void Start()
    {
        StartCoroutine(FallingCheck());
    }

    private void FixedUpdate()
    {

    }
    IEnumerator FallingCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            previousVelocity = player.rb.velocity.y;
            if (player.rb.velocity.y > 0.1f)
            {
                Debug.Log("Jumping");
                falling = false;
            }
            else if (player.rb.velocity.y < -0.1f)
            {
                Debug.Log("Falling");
                falling = true;
            }
            else if (player.rb.velocity.y == 0)
            {
                Debug.Log("Stopped");
                falling = false;
            }
        }
    }

    public void GroundCheck()
    {
        rayCastOrigin = (transform.position - new Vector3(0, 0.65f, 0));
        Debug.DrawRay(rayCastOrigin, (Vector3.down) * groundCheckRayLength, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, groundCheckRayLength))
        {
            if (falling == true)
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    distanceToGround = hit.distance;
                    player.rb.velocity = new Vector3(player.rb.velocity.x, 0, 0);
                    if (distanceToGround >= 0 && distanceToGround <= 0.18f)
                    {
                        transform.position = new Vector3(hit.point.x, hit.point.y + 0.85f,0);
                    }


                    player.grounded = true;
                }
            }
        }
        else if (!Physics.Raycast(rayCastOrigin - new Vector3(0, 0.75f, 0), Vector3.down, out hit, 0.15f))
        {
            player.grounded = false;
            return;
        }
    }
}
