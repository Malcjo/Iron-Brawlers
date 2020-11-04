using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasts : MonoBehaviour
{

    [SerializeField] private LayerMask groundMask = 1 << 12;
    [SerializeField] private LayerMask waterMask = 1 << 4;
    [SerializeField] private int groundLayer = 12;
    [SerializeField] Player player;
    [SerializeField] PlayerInput playerInput;

    private float groundCheckRayLength = 0.4f;
    private float headCheckRayLength = 0.5f;
    private float sideCheckRayLength = 0.3f;


    public bool debugModeOn = true;

    public ParticleSystem splashParticle;



    private void FixedUpdate()
    {

        PublicRayCasting();
    }
    public void PublicRayCasting()
    {
        DownRays();
        UpRays();
        SideRayCaster();
        DebugMode();
    }
    void DebugMode()
    {
        if (debugModeOn == true)
        {
            Vector3 rayCastOrigin = transform.position;
            Debug.DrawRay(rayCastOrigin, Vector3.left * sideCheckRayLength, Color.red);
            Debug.DrawRay(rayCastOrigin + new Vector3(0, 0.5f, 0), Vector3.left * sideCheckRayLength, Color.red);
            Debug.DrawRay(rayCastOrigin + new Vector3(0, -0.7f, 0), Vector3.left * sideCheckRayLength, Color.red);

            Debug.DrawRay(rayCastOrigin, Vector3.right * sideCheckRayLength, Color.red);
            Debug.DrawRay(rayCastOrigin + new Vector3(0, 0.5f, 0), Vector3.right * sideCheckRayLength, Color.red);
            Debug.DrawRay(rayCastOrigin + new Vector3(0, -0.7f, 0), Vector3.right * sideCheckRayLength, Color.red);
        }
    }
    void SideRayCaster()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;

        if (Physics.Raycast(rayCastOrigin, Vector3.left, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3 (0,0.5f,0), Vector3.left, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3(0, -0.7f, 0), Vector3.left, out hit, sideCheckRayLength, groundMask))
        {
            player.RayCasterLeftWallCheck(hit);
        }
        else if (Physics.Raycast(rayCastOrigin, Vector3.right, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3(0, 0.5f, 0), Vector3.right, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3(0, -0.7f, 0), Vector3.right, out hit, sideCheckRayLength, groundMask))
        {
            player.RayCasterRightWallCheck(hit);
        }
        else
        {
            player.SetCurrentWallNone();
        }
    }
    


    //--------------------------------------------------------------------------------
    private void UpRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Debug.DrawRay(rayCastOrigin, Vector3.up * headCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.up, out hit, headCheckRayLength, groundMask))
        {
            player.RayCastCeilingCheck(hit);
        }
    }

    //--------------------------------------------------------------------------------
    private void DownRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position - new Vector3(0, 0.45f, 0);
        Debug.DrawRay(rayCastOrigin, Vector3.down * groundCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, groundCheckRayLength, groundMask))
        {
            player.RaycastGroundCheck(hit);
        }
        else if (!Physics.Raycast(rayCastOrigin - new Vector3(0, 0.75f, 0), Vector3.down, out hit, 0.15f))
        {
            player.PlayerGroundedFalse();
        }

    }

}
