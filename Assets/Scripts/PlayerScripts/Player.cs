﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float weight = 10;


    [SerializeField] private float knockbackResistance = 5;
    [SerializeField] Vector3 gravity = new Vector3 (0,-9.81f,0);

    public bool canHitBox;
    public bool inAnimation;
    public bool grounded;
    public bool hasArmour;
    public bool hitStun;
    private float hitStunCounter;
    public bool blocking;
    public bool canJump;

    [SerializeField] public bool jumping;
    [SerializeField] public int numberOfJumps;
    private int maxJumps = 2;


    public GameObject armour;
    public Rigidbody rb;
    private PlayerInput playerInput;
    private ArmourCheck armourCheck;
    private PlayerControls playerControls;
    private Checker checker;
    private Raycasts raycasts;
    [SerializeField] private PlayerStats playerStats;

    private Vector3 addForceValue;
    private Vector3 hitDirection;

    public Transform[] characterJoints;

    public int lives;
    public int maxLives = 3;

    public float friction = 0.25f;
    private void Awake()
    {
        checker = GetComponent<Checker>();
        raycasts = GetComponent<Raycasts>();
        playerControls = GetComponent<PlayerControls>();
        armourCheck = GetComponent<ArmourCheck>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
    }
    void Start()
    {
        numberOfJumps = maxJumps;
        blocking = false;
        lives = maxLives;
        canHitBox = true;
        inAnimation = false;
        armourCheck.SetAllArmourOff();
    }
    private void Update()
    {
        ReduceCounter();

    }
    private void FixedUpdate()
    {
        checker.BoundsChecker();
        raycasts.PublicRayCasting();
        Move();
        Gravity();
    }

    void Move()
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
                rb.velocity = new Vector3(playerInput.horizontal * playerStats.CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
            }
        }
        else
        {
            rb.velocity = new Vector3(playerInput.horizontal * playerStats.CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
        }
    }

    void Gravity()
    {
        if (grounded == false)
        {
            gravity = new Vector3 (0, -9.81f, 0);
            rb.AddForce((gravity * ((weight + armourCheck.armourWeight) / 10)));
        }
        else if(grounded == true)
        {

            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            gravity = Vector3.zero;
        }
    }
    public void Jump()
    {
        if (playerInput.numberOfJumps > 0)
        {
            grounded = false;
            rb.velocity = (new Vector3(rb.velocity.x, playerStats.JumpForceCalculator(), rb.velocity.z));
            jumping = true;
            numberOfJumps--;
        }
    }
    public float SetVelocityY()
    {
        return rb.velocity.y;
    }

    #region ReduceValues
    void ReduceCounter()
    {
        ReduceHit();
        ReduceHitStun();
    }

    void ReduceHit()
    {
        addForceValue.x = Mathf.Lerp(addForceValue.x, 0, 5f * Time.deltaTime);
        addForceValue.y = Mathf.Lerp(addForceValue.y, 0, 25f * Time.deltaTime);

        //reducing to zero if small value
        if (addForceValue.magnitude < 0.05f && addForceValue.magnitude > -0.05f)
        {
            addForceValue = Vector3.zero;
        }
    }
    void ReduceHitStun()
    {
        if (hitStun == true)
        {
            hitStunCounter -= 1 * Time.deltaTime;
            if (hitStunCounter < 0.001f)
            {
                hitStunCounter = 0;
                hitStun = false;
            }
        }
    }
    #endregion
    private Vector3 AddForce(float hitStrength)
    {
        Vector3 addForceValue = ((hitDirection) * (hitStrength));
        return addForceValue;
    }
    #region Colliders / Triggers
    #region Groud Detection
    //Ground Detections----------------------------------------------------------
    //resetting number of jumps to max jumps
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerInput.numberOfJumps = playerInput.maxJumps;
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
    //Ground Detections----------------------------------------------------------
    #endregion

    private void OnTriggerExit(Collider other)
    {
        canHitBox = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            return;
        }
        if (other.tag == "Jab")
        {
            if (canHitBox == false)
            {
                Debug.Log("canhitbox is false");
                return;
            }

            else if (canHitBox == true)
            {
                Debug.Log("canhitbox is true");
                if (blocking == true)
                {
                    return;
                }
                else if (blocking == false)
                {
                    canHitBox = false;
                    Debug.Log("Jab");
                    hitStun = true;
                    hitStunCounter = 1.1f;
                    Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection(); // getting the direction of the attack
                    float Power = other.GetComponent<TempHitBox>().HitStrength(); // getting the power of the attack

                    hitDirection = Hit;
                    addForceValue = AddForce(Power - (armourCheck.knockBackResistance + knockbackResistance));
                }
            }
        }
    }
    #endregion
    #region not being used
    public float GetLowestYValue(Transform[] arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].transform.position.y < value)
            {
                index = i;
                value = arr[i].transform.position.y;
            }
        }
        return index;
    }
    #endregion
}

