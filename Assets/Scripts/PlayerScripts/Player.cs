using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float weight = 10;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private float speed = 8;
    [SerializeField] private float knockbackResistance = 5;

    public bool canHitBox;
    public bool inAnimation;
    public bool grounded;
    public bool hasArmour;
    public bool hitStun;
    private float hitStunCounter;
    public bool blocking;

    [SerializeField] private bool jumping;
    [SerializeField] private int numberOfJumps;
    private int maxJumps = 2;


    public GameObject armour;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private ArmourCheck armourCheck;
    private PlayerControls playerControls;

    private Vector3 addForceValue;
    private Vector3 hitDirection;

    public int lives;
    private int maxLives = 3;

    public float friction = 0.25f;
    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        armourCheck = GetComponent<ArmourCheck>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
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
        if (hitStun == true)
        {
            characterSpeed *= 0 + (5 * Time.deltaTime);
        }
        return characterSpeed;
    }
    void Gravity()
    {
        rb.AddForce(Physics.gravity * ((weight + armourCheck.armourWeight) / 10));
    }
    public void Jump()
    {
        if (playerInput.numberOfJumps > 0)
        {
            rb.velocity = (new Vector3(rb.velocity.x, JumpForceCalculator(), rb.velocity.z));
            jumping = true;
            numberOfJumps--;
        }
    }
    float JumpForceCalculator()
    {
        float jumpForceValue;
        if (numberOfJumps == 0)
        {
            return rb.velocity.y;
        }
        if(jumping == false)
        {
            jumpForceValue = jumpForce - armourCheck.reduceJumpForce;
            return jumpForceValue;
        }
        jumpForceValue = (jumpForce + 1) - armourCheck.reduceJumpForce;
        return jumpForceValue;
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
    #region RemoveArmour
    public void RemoveLegArmour()
    {
        armourCheck.LegArmourType = ArmourCheck.Armour.none;
        armourCheck.SetArmourOff(ArmourCheck.ArmourType.Legs);
    }
    public void RemoveChestArmour()
    {
        armourCheck.ChestArmourType = ArmourCheck.Armour.none;
        armourCheck.SetArmourOff(ArmourCheck.ArmourType.Chest);
    }
    #endregion
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
        var playerChecker = other.GetComponentInParent<PlayerControls>();
        if (playerChecker.playerNumber == playerControls.playerNumber)
        {
            Debug.Log("is same as player");
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
                    hitStunCounter = 0.8f;
                    Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection(); // getting the direction of the attack
                    float Power = other.GetComponent<TempHitBox>().HitStrength(); // getting the power of the attack

                    hitDirection = Hit;
                    addForceValue = AddForce(Power - (armourCheck.knockBackResistance + knockbackResistance));
                    armourCheck.LegArmourType = ArmourCheck.Armour.none;
                }
            }
        }
    }
    #endregion
}
