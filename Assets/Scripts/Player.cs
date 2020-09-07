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

    void Start()
    {
        hasArmour = true;
        armourStats = GetComponent<ArmourStats>();
        attack = GetComponent<PlayerAttack>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerInput.JumpInput();
        if(hasArmour == false)
        {
            armour.SetActive(false);
            armourWeight = 0;
            armourReduceKnockback = 0;
            armourReduceSpeed = 0;
        }
        else
        {
            armour.SetActive(true);
            armourWeight = armourStats.armourWeightStat();
            armourReduceKnockback = armourReduceKnockbackStat;
            armourReduceSpeed = armourStats.armourSpeedReduceStat();
        }

    }
    private void FixedUpdate()
    {
        playerInput.HorizontalInput();
        MoveCall();
        Gravity();
        JumpMove();
        playerInput.AttackInput();
    }
    void MoveCall()
    { 
        //Vector3.right * playerInput.horizontal * (speed);
        rb.velocity = new Vector3(playerInput.horizontal* (speed - armourReduceSpeed), rb.velocity.y,0);
    }

    void JumpMove()
    {
        if(playerInput.canJump> 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = (new Vector3(rb.velocity.x, jumpForce, rb.velocity.z));
            }
        }
    }

    void Gravity()
    {
        rb.AddForce(Physics.gravity * ((weight + armourWeight) / 10));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            playerInput.canJump = playerInput.maxJumps;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerInput.canJump++;
            }
            playerInput.canJump--;
        }
    }
}
