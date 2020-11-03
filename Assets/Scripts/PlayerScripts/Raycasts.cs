﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasts : MonoBehaviour
{

    private float groundCheckRayLength = 0.4f;
    private float headCheckRayLength = 0.5f;
    private float sideCheckRayLength = 0.3f;

    [SerializeField] private LayerMask groundMask = 1<<12;
    [SerializeField] private LayerMask waterMask = 1<<4;
    [SerializeField] private int groundLayer = 12;

    private float distanceToGround;
    private float distanceToCeiling;
    private float distanceToRight;
    private float distanceToLeft;

    public bool debugModeOn = true;

    public ParticleSystem splashParticle;

    Player player;
    PlayerInput playerInput;



    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {

        PublicRayCasting();
    }
    public void PublicRayCasting()
    {
        DownRays();
        UpRays();
        SideRayChecker();
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
    void SideRayChecker()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;

        if (Physics.Raycast(rayCastOrigin, Vector3.left, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3 (0,0.5f,0), Vector3.left, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3(0, -0.7f, 0), Vector3.left, out hit, sideCheckRayLength, groundMask))
        {
            if (player.jumping == true || player.falling == true || player.grounded == true)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    playerInput.CurrentWall = PlayerInput.Wall.leftWall;
                    HitLeft(hit);
                }
            }
        }
        else if (Physics.Raycast(rayCastOrigin, Vector3.right, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3(0, 0.5f, 0), Vector3.right, out hit, sideCheckRayLength, groundMask) || 
            Physics.Raycast(rayCastOrigin + new Vector3(0, -0.7f, 0), Vector3.right, out hit, sideCheckRayLength, groundMask))
        {
            if (player.jumping == true || player.falling == true || player.grounded == true)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    playerInput.CurrentWall = PlayerInput.Wall.rightWall;
                    HitRight(hit);
                }
            }
        }
        else
        {
            playerInput.CurrentWall = PlayerInput.Wall.none;
        }
    }
    void HitLeft(RaycastHit hit)
    {
        distanceToLeft = hit.distance;
        if (distanceToLeft >= 0 && distanceToLeft <= 0.2f)
        {
            if (player.rb.velocity.x > 0)
            {
                player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
            }
        }
        distanceToLeft = hit.distance;
    }
    void HitRight(RaycastHit hit)
    {
        distanceToRight = hit.distance;
        if (distanceToRight >= 0 && distanceToRight <= 0.2f)
        {
            if(player.rb.velocity.x > 0)
            {
                player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
            }
        }
        distanceToRight = hit.distance;
    }
    //--------------------------------------------------------------------------------
    private void UpRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Debug.DrawRay(rayCastOrigin, Vector3.up * headCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.up, out hit, headCheckRayLength, groundMask))
        {
            if(player.jumping == true)
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
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.9f, 0);
        }
        distanceToCeiling = hit.distance;
    }
    //--------------------------------------------------------------------------------
    private void DownRays()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position - new Vector3(0, 0.45f, 0);
        Debug.DrawRay(rayCastOrigin, Vector3.down * groundCheckRayLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, groundCheckRayLength, groundMask))
        {
            if (player.falling == true)
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
    private void LandOnGround(RaycastHit hit)
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
