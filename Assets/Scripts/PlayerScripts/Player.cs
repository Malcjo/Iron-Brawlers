using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] Vector3 gravity = new Vector3 (0,-9.81f,0);

    public bool canHitBox;
    public bool inAnimation;
    public bool grounded;
    public bool hasArmour;
    public bool hitStun;
    private float hitStunCounter;
    public bool blocking;
    public bool canJump;
    public bool canAirMove;
    private bool reduceAddForce;

    public PlayerIndex playerNumber;
    public int characterType;

    public bool DebugModeOn;

    [SerializeField] private float reduceCounterValue;
    [SerializeField] private float reduceCounterMax;


    [SerializeField] public bool jumping;
    [SerializeField] public int numberOfJumps;
    private int maxJumps = 2;

    public Rigidbody rb;

    private PlayerInput playerInput;
    private ArmourCheck armourCheck;
    private Checker checker;
    private Raycasts raycasts;
    [SerializeField] private PlayerStats playerStats;

    private Vector3 addForceValue;
    private Vector3 hitDirection;

    public Transform[] characterJoints;
    public float facingDirection;

    [SerializeField] private float VisualVelocity;
    [SerializeField] private float YVelocity;

    public int lives;
    public int maxLives = 3;

    public float friction = 0.25f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        checker = GetComponent<Checker>();
        raycasts = GetComponent<Raycasts>();
        armourCheck = GetComponent<ArmourCheck>();
        playerInput = GetComponent<PlayerInput>();
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
        jumping = checker.jumping;
        hasArmour = armourCheck.HasArmour(); ;
        ReduceCounter();
    }
    private void FixedUpdate()
    {
        VisualVelocity = rb.velocity.magnitude;
        YVelocity = rb.velocity.y;
        Move();
        Gravity();
        if(rb.velocity.y < -20)
        {
            rb.velocity = new Vector3(playerInput.horizontal * playerStats.CharacterSpeed(), -20, 0) + addForceValue;
        }
    }

    void Move()
    {
        if (inAnimation == true)
        {
            if(grounded == true)
            {
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
                return;
            }
            else if (grounded == false)
            {
                if (canAirMove == false)
                {
                    rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
                    return;
                }
                else if (canAirMove == true)
                {
                    rb.velocity = new Vector3(playerInput.horizontal * playerStats.CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
                }
            }
        }
        else
        {
            rb.velocity = new Vector3(playerInput.horizontal * playerStats.CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
        }
    }
    public void Roll()
    {

    }
    void Gravity()
    {
        if (grounded == false)
        {
            gravity = new Vector3 (0, -9.81f, 0);
            rb.AddForce((gravity * ((playerStats.weight + armourCheck.armourWeight) / 10)));
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
        addForceValue.x = Mathf.Lerp(addForceValue.x, 0, 7f * Time.deltaTime);
        addForceValue.y = Mathf.Lerp(addForceValue.y, 0, 27f * Time.deltaTime);

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
    }
    public void Damage(Vector3 Hit, float Power)
    {
        Debug.Log("Jab");
        hitStun = true;
        hitStunCounter = 1.1f;
        hitDirection = Hit;
        addForceValue = AddForce(Power - (armourCheck.knockBackResistance + playerStats.knockbackResistance));
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

