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
    public Vector3 testAddForce;


    private float hitDirectionX;
    private float hitDirectionY;

    private Vector3 hitDirection;

    public float knockbackResistance;


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
        testAddForce = AddForce();
    }

    private void FixedUpdate()
    {
        MoveCall();
        Gravity();
        ReduceHit();
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
                rb.velocity = new Vector3(playerInput.horizontal * CharacterSpeed(), rb.velocity.y, 0) + AddForce(); ;
            }
        }
        else
        {
            rb.velocity = new Vector3(playerInput.horizontal * CharacterSpeed(), rb.velocity.y, 0) + AddForce();
        }
    }

    private float CharacterSpeed()
    {
        float characterSpeed = speed - armourStats.armourReduceSpeed;
        return characterSpeed;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jab")
        {
            Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection();
            hitDirectionX = Hit.x;
            hitDirectionY = Hit.y;


            if (hasArmour == true)
            {
                Debug.Log("Hit Armour!");
                return;
            }
            else if (hasArmour == false)
            {
                Debug.Log("Hit Body!");
                hitDirection = Hit;
            }
            knockbackResistance = 0;
        }
    }
    void ReduceHit()
    {
        if(hitDirection.x > 0)
        {
            hitDirection.x = Mathf.Lerp(hitDirection.x, 0, hitDirectionX / 15);
            hitDirection.y = Mathf.Lerp(hitDirection.y, 0, hitDirectionY * 3f);
        }
        else
        {
            hitDirection.x = Mathf.Lerp(hitDirection.x, 0, hitDirectionX / 15);
            hitDirection.y = Mathf.Lerp(hitDirection.y, 0, hitDirectionY * 3f);
        }

    }

    private Vector3 AddForce()
    {
        Vector3 addForceValue = ((hitDirection) * (15f - armourStats.knockBackResistance));
        return addForceValue;
    }

    public void Knockback(Vector3 direction, float knockbackResistance)
    {
        rb.AddForce(direction * (250 - knockbackResistance));
    }
}
