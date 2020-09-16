using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float weight;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;

    public bool inAnimation;
    public bool grounded;
    public bool hasArmour;

    public GameObject armour;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private ArmourCheck armourCheck;

    private Vector3 addForceValue;
    private Vector3 hitDirection;

    private float friction = 0.25f;


    void Start()
    {
        inAnimation = false;
        hasArmour = false;
        armourCheck = GetComponent<ArmourCheck>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        Gravity();
        ReduceHit();
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
        float characterSpeed = speed - armourCheck.armourReduceSpeed;
        return characterSpeed;
    }
    public void Jump()
    {
        if (playerInput.canJump > 0)
        {
            rb.velocity = (new Vector3(rb.velocity.x, (jumpForce - armourCheck.reduceJumpForce), rb.velocity.z));
        }
    }
    void Gravity()
    {
        rb.AddForce(Physics.gravity * ((weight + armourCheck.armourWeight) / 10));
    }
    void ReduceHit()
    {
        addForceValue.x = Mathf.Lerp(addForceValue.x, 0, 5f * Time.deltaTime);
        addForceValue.y = Mathf.Lerp(addForceValue.y, 0, 50f * Time.deltaTime);
        //reducing to zero if small value
        if (addForceValue.magnitude < 0.05f && addForceValue.magnitude > -0.05f)
        {
            addForceValue = Vector3.zero;
        }
    }
    private Vector3 AddForce(float hitStrength)
    {
        Vector3 addForceValue = ((hitDirection) * (hitStrength));
        return addForceValue;
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
            Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection(); // getting the direction of the attack
            float Power = other.GetComponent<TempHitBox>().HitStrength(); // getting the power of the attack

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

}
