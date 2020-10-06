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
    private Rigidbody rb;
    private PlayerInput playerInput;
    private ArmourCheck armourCheck;
    private PlayerControls playerControls;
    [SerializeField] private PlayerStats playerStats;

    private Vector3 addForceValue;
    private Vector3 hitDirection;
    private Vector3 rayCastOrigin;
    public Vector3 rayCastOffset;
    public Transform[] characterJoints;

    [SerializeField] private int groundLayer;

    public int lives;
    public int maxLives = 3;

    public float friction = 0.25f;
    private void Awake()
    {
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
        rayCastOrigin = transform.position;
        GroundCheck();
        Debug.DrawRay(rayCastOrigin, (-Vector3.down) * .65f, Color.green);
        Debug.DrawRay(rayCastOrigin, (Vector3.down) * .85f, Color.green);

        Debug.DrawRay(rayCastOrigin, (-Vector3.right) * .2f, Color.green);
        Debug.DrawRay(rayCastOrigin, (Vector3.right) * .2f, Color.green);

        Move();

        Gravity();

    }
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
    void GroundCheck()
    {
        groundLayer = 1 << 12;
        Debug.DrawRay(rayCastOrigin - new Vector3(0, 0.75f, 0), (Vector3.down) * .15f, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(rayCastOrigin - new Vector3(0, 0.75f, 0), Vector3.down, out hit, 0.15f))
        {
            if(hit.collider.gameObject.CompareTag("Ground"))
            {
                grounded = true;
                return;
            }
        }
        else if(!Physics.Raycast(rayCastOrigin - new Vector3(0, 0.75f, 0), Vector3.down, out hit, 0.15f))
        {
            grounded = false;
            return;
        }

    }
    void Gravity()
    {
        if (grounded == false)
        {
            gravity = new Vector3 (0, -9.81f, 0);
            rb.AddForce((gravity * ((weight + armourCheck.armourWeight) / 10)));
            Debug.Log("Not Grounded");
        }
        else if(grounded == true)
        {
            gravity = Vector3.zero;
            Debug.Log("Grounded");
        }

    }
    public void Jump()
    {
        if (playerInput.numberOfJumps > 0)
        {
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
}

