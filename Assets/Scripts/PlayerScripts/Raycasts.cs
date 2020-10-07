using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasts : MonoBehaviour
{

    private float groundCheckRayLength = 0.4f;
    private float headCheckRayLength = 0.5f;
    private float sideCheckRayLength = 0.2f;


    public float distanceToGround;
    public float distanceToCeiling;
    public float distanceToRight;
    public float distanceToLeft;

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
        LeftRays();
        RightRays();

    }

    private void LeftRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Debug.DrawRay(rayCastOrigin, Vector3.left * sideCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.left, out hit, sideCheckRayLength))
        {
            if(checker.jumping == true || checker.falling == true)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    HitLeft(hit);
                }
            }
        }
    }
    void HitLeft(RaycastHit hit)
    {
        distanceToLeft = hit.distance;
        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, 0);
        if (distanceToLeft >= 0 && distanceToLeft <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x + 0.25f, hit.point.y, 0);
        }
        distanceToLeft = hit.distance;
    }
    //--------------------------------------------------------------------------------
    private void RightRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Debug.DrawRay(rayCastOrigin, Vector3.right * sideCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.right, out hit, sideCheckRayLength)) 
        {
            if (checker.jumping == true || checker.falling == true) 
            {
                HitRight(hit);
            } 
        }
    }
    void HitRight(RaycastHit hit)
    {
        distanceToRight = hit.distance;
        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, 0);
        if (distanceToRight >= 0 && distanceToRight <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x - 0.25f, hit.point.y, 0);
        }
        distanceToRight = hit.distance;
    }
    //--------------------------------------------------------------------------------
    private void UpRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Debug.DrawRay(rayCastOrigin, Vector3.up * headCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.up, out hit, headCheckRayLength))
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
    //--------------------------------------------------------------------------------
    private void DownRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position - new Vector3(0, 0.45f, 0);
        Debug.DrawRay(rayCastOrigin, Vector3.down * groundCheckRayLength, Color.red);
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
