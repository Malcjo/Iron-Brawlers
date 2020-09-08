using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float weight;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;

    [SerializeField] public bool grounded;
    [SerializeField] public GameObject armour;


    public Rigidbody rb;
    [SerializeField] public PlayerAttack attack;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ArmourStats armourStats;

    [SerializeField] public float armourWeight;
    [SerializeField] public float armourReduceSpeed;
    [SerializeField] public float armourReduceKnockback;

    [SerializeField] public float armourReduceSpeedStat;
    [SerializeField] public float armourReduceKnockbackStat;

    [SerializeField] public bool hasArmour;

    public PlayerIndex PlayerNumber;
    public enum PlayerIndex { Player1, Player2 };

    void Start()
    {
        hasArmour = false;
        armourStats = GetComponent<ArmourStats>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ArmourCheck();
    }

    private void FixedUpdate()
    {
        MoveCall();
        Gravity();
    }


    void MoveCall()
    { 
        rb.velocity = new Vector3(playerInput.horizontal* (speed - armourReduceSpeed), rb.velocity.y,0);
    }

    public void JumpMove()
    {
        if(playerInput.canJump> 0)
        {
            rb.velocity = (new Vector3(rb.velocity.x, (jumpForce - armourStats.reduceJumpForce ), rb.velocity.z));
        }
    }

    void Gravity()
    {
        rb.AddForce(Physics.gravity * ((weight + armourWeight) / 10));
    }

    void ArmourCheck()
    {
        if (hasArmour == false)
        {
            armour.SetActive(false);
            armourWeight = 0;
            armourReduceKnockback = 0;
            armourReduceSpeed = 0;
        }
        else
        {
            armour.SetActive(true);
            armourWeight = armourStats.armourWeight;
            armourReduceKnockback = armourReduceKnockbackStat;
            armourReduceSpeed = armourStats.armourReduceSpeed;
        }
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
        else
        {
            grounded = false;
        }
    }
}
