using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasts : MonoBehaviour
{

    private Vector3 rayCastOrigin;
    public Vector3 rayCastOffset;

    private float groundCheckRayLength = 0.4f;
    private float headCheckRayLength = 0.7f;


    public float distanceToGround;
    public float distanceToCeiling;

    Checker checker;
    Player player;
    private void Awake()
    {
        checker = GetComponent<Checker>();
        player = GetComponent<Player>();
    }

    public void PublicRayCasting()
    {
        DownRays();
        UpRays();
    }
    private void UpRays()
    {
        Debug.DrawRay(transform.position, Vector3.up * headCheckRayLength, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(rayCastOrigin, Vector3.up, out hit, groundCheckRayLength))
        {
            if(checker.jumping == true)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    HitCeiling(hit);
                }
            }
        }
    }
    private void DownRays()
    {
        rayCastOrigin = (transform.position - new Vector3(0, 0.45f, 0));

        Debug.DrawRay(rayCastOrigin, Vector3.down * groundCheckRayLength, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, groundCheckRayLength))
        {

            if (checker.falling == true)
            {
                if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Platform")))
                {
                    LandOnGround(hit);
                }
            }
        }
        else if (!Physics.Raycast(rayCastOrigin - new Vector3(0, 0.75f, 0), Vector3.down, out hit, 0.15f))
        {
            player.grounded = false;
            return;
        }
    }
    void HitCeiling(RaycastHit hit)
    {
        distanceToCeiling = hit.distance;
        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, 0);
        if (distanceToCeiling >= 0 && distanceToCeiling <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.85f, 0);
        }
        distanceToCeiling = hit.distance;

    }
    void LandOnGround(RaycastHit hit)
    {
        distanceToGround = hit.distance;
        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, 0);
        if (distanceToGround >= 0 && distanceToGround <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.85f, 0);
        }
        distanceToGround = hit.distance;

        player.grounded = true;
    }
}
