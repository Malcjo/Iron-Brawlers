﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float weight;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;

    [SerializeField] public bool grounded;
    [SerializeField] public GameObject armour;

    public float friction;

    public Rigidbody rb;
    [SerializeField] public HitBoxManager attack;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ArmourStats armourStats;

    /*
    [SerializeField] public float armourWeight;
    [SerializeField] public float armourReduceSpeed;
    [SerializeField] public float armourReduceKnockback;

    [SerializeField] public float armourReduceSpeedStat;
    [SerializeField] public float armourReduceKnockbackStat;
    */
    [SerializeField] public bool hasArmour;

    public bool inAnimation;

    void Start()
    {
        inAnimation = false;
        hasArmour = false;
        armourStats = GetComponent<ArmourStats>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        MoveCall();
        Gravity();
    }


    void MoveCall()
    {
        if (inAnimation == true)
        {
            if (grounded == true)
            {
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
                return;
            }
            else if (grounded == false)
            {
                rb.velocity = new Vector3(playerInput.horizontal * (speed - armourStats.armourReduceSpeed), rb.velocity.y, 0);
            }
        }
        else
        {
            rb.velocity = new Vector3(playerInput.horizontal * (speed - armourStats.armourReduceSpeed), rb.velocity.y, 0);
        }
    }

    public void JumpMove()
    {
        if (playerInput.canJump > 0)
        {
            rb.velocity = (new Vector3(rb.velocity.x, (jumpForce - armourStats.reduceJumpForce), rb.velocity.z));
        }
    }

    void Gravity()
    {
        rb.AddForce(Physics.gravity * ((weight + armourStats.armourWeight) / 10));
    }


    //Collision Detections----------------------------------------------------------
    //resetting number of jumps to max jumps
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerInput.canJump = playerInput.maxJumps;
        }
    }
    //checking if grounded
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
            
    }
}
