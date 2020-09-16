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

    public float friction;

    public Rigidbody rb;
    [SerializeField] public HitBoxManager attack;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ArmourStats armourStats;
    [SerializeField] private Vector3 addForceValue;

    public Vector3 testAddForce;
    public float testDirX;

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
                rb.velocity = new Vector3(playerInput.horizontal * CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
            }
        }
        else
        {
            rb.velocity = new Vector3(playerInput.horizontal * CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
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
            var otherScript = other.GetComponent<TempHitBox>();
            Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection();
            float Power = other.GetComponent<TempHitBox>().HitStrength();

            if (hasArmour == true)
            {
                return;
            }
            else if (hasArmour == false)
            {
                hitDirection = Hit;
                addForceValue = AddForce(Power);
            }
        }
    }
    void ReduceHit()
    {
        addForceValue.x = Mathf.Lerp(addForceValue.x, 0, 5f * Time.deltaTime);
        addForceValue.y = Mathf.Lerp(addForceValue.y, 0, 50f * Time.deltaTime);
        if(addForceValue.magnitude < 0.1f && addForceValue.magnitude > -0.1f)
        {
            addForceValue = Vector3.zero;
        }
    }

    private Vector3 AddForce(float hitStrength)
    {
        Vector3 addForceValue = ((hitDirection) * (hitStrength));
        return addForceValue;
    }
}
