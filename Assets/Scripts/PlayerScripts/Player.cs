using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerIndex { Player1, Player2 };

    public PlayerIndex playerNumber;
    [SerializeField] private VState currentVerticalState;

    [SerializeField]
    private string CurrentStateName;

    [SerializeField]
    public float gravityValue = -10f;
    [SerializeField]
    private float friction = 0.25f;
    [SerializeField]
    private int maxLives = 3;



    [SerializeField] private float speed = 6.5f;

    [SerializeField] private float weight = 22;
    [SerializeField] private float knockbackResistance = 3;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField] 
    private PlayerInput playerInput;
    [SerializeField] 
    private ArmourCheck armourCheck;
    [SerializeField] 
    private Raycasts raycasts;
    [SerializeField] 
    private PlayerActions playerActions;
    [SerializeField]
    private PlayerSetup playerSetup;

    [Header("UI")]
    [SerializeField]
    public TMP_Text playerLives;
    [SerializeField]
    public Image playerImage;

    [Header("Observation Values")]
    [SerializeField]
    private float CurrentVelocity;
    [SerializeField]
    private float YVelocity;
    private float previousVelocity;
    [SerializeField]
    private Vector3 V3Velocity;

    public bool DebugModeOn;

    private float distanceToGround;
    private float distanceToCeiling;
    private float distanceToRight;
    private float distanceToLeft;

    private bool canHitBox;
    private bool hasArmour;
    private bool hitStun;
    private bool blocking;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canAirMove;
    private bool reduceAddForce;
    [SerializeField]
    private bool gravityOn;
    [SerializeField] private bool canTurn;
    private bool canDoubleJump;

    [SerializeField] private float jumpForce = 9;
    private float hitStunTimer;


    [SerializeField] private int currentJumpIndex;
    [SerializeField] private int maxJumps = 2;
    public int facingDirection;

    private Vector3 addForceValue;
    private Vector3 hitDirection;
    public int lives;
    public int characterType;

    private Wall currentWall;

    private PlayerState MyState;

    public enum Wall
    {
        leftWall,
        rightWall,
        none
    }
    public enum VState
    {
        grounded,
        jumping,
        falling
    }
    void Start()
    {
        MyState = new IdleState();
        gravityOn = true;
        canTurn = true;
        canAirMove = true;
    }
    private void Update()
    {
        CheckDirection();
        CharacterStates();
    }
    private void FixedUpdate()
    {
        Observation();
        GravityCheck();
    }

    #region State Machine
    void CharacterStates()
    {
        VerticalState();
        CurrentStateName = MyState.GiveName();
        MyState.RunState(this, playerInput.GetHorizontal(), playerInput.ShouldAttack(), playerInput.ShouldJump(), playerInput.ShouldCrouch());
    }

    public void SetState(PlayerState state)
    {
        MyState = state;
    }

    public void RunIdleState()
    {
        rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
        playerActions.Idle();
    }

    public void RunJumpingState()
    {
        if (currentJumpIndex < maxJumps)
        {
            rb.velocity = (new Vector3(rb.velocity.x, JumpForceCalculator(), rb.velocity.z)) + addForceValue;
        }
    }
    void DoubleJumpCheck()
    {
        if (canDoubleJump == true)
        {
            if (currentVerticalState == VState.falling || currentVerticalState == VState.jumping)
            {
                canTurn = true;
                playerActions.DoubleJump(true);
            }
        }
        canDoubleJump = false;
    }
    public void RunCrouchingState()
    {
        rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
        playerActions.Crouching();
    }
    public void RunMoveState()
    {
        rb.velocity = new Vector3(playerInput.GetHorizontal() * CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
        playerActions.Running();
    }
    public void RunAirborneMoveState()
    {
        if (canAirMove == false)
        {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
            return;
        }
        else if (canAirMove == true)
        {
            rb.velocity = new Vector3(playerInput.GetHorizontal() * CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
        }
    }
    public void RunJabState()
    {
        playerActions.JabCombo();
        Debug.Log("Jab");
    }

    void RunBusyState()
    {

    }




    void VerticalState()
    {
        playerActions.VerticalAnim();
        if (rb.velocity.y != 0)
        {
            if (rb.velocity.y > 0.1f)
            {
                currentVerticalState = VState.jumping;
            }
            else if (rb.velocity.y < -0.1f)
            {
                currentVerticalState = VState.falling;
            }
        }
    }
    #endregion

    #region Gravity Group
    void TerminalVelocity()
    {
        if (rb.velocity.y < -20)
        {
            rb.velocity = new Vector3(playerInput.GetHorizontal() * CharacterSpeed(), -20, 0) + addForceValue;
        }
    }
    void GravityCheck()
    {
        if (gravityOn == false)
        {
            return;
        }
        else if (gravityOn == true)
        {
            Gravity();
        }
    }
    void Gravity()
    {
        TerminalVelocity();
        if (currentVerticalState == VState.falling || currentVerticalState == VState.jumping)
        {
            gravityValue = 10;
            rb.AddForce(Vector3.down * gravityValue * ((weight + armourCheck.armourWeight) / 10));
        }
        else if (currentVerticalState == VState.grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            gravityValue = 0;
        }
    }

    #endregion

    #region Damaging
    public void HitStun()
    {
        playerActions.HitStun();
    }
    private Vector3 AddForce(float hitStrength)
    {
        Vector3 addForceValue = ((hitDirection) * (hitStrength));
        return addForceValue;
    }
    public void Damage(Vector3 Hit, float Power)
    {
        Debug.Log("Jab");
        hitStun = true;
        hitStunTimer = 1.1f;
        hitDirection = Hit;
        addForceValue = AddForce(Power - (armourCheck.knockBackResistance + knockbackResistance));
    }
    #endregion
    public float CharacterSpeed()
    {
        float characterSpeed = speed - armourCheck.armourReduceSpeed;
        if (hitStun == true)
        {
            characterSpeed *= 0 + (5 * Time.deltaTime);
        }
        return characterSpeed;
    }
    
    public float JumpForceCalculator()
    {
        float jumpForceValue;
        if (currentJumpIndex == maxJumps)
        {
            return rb.velocity.y;
        }
        else if (currentJumpIndex < maxJumps)
        {
            if (currentVerticalState == VState.grounded)
            {
                jumpForceValue = jumpForce - armourCheck.reduceJumpForce;
                return jumpForceValue;
            }
            else if (currentVerticalState == VState.jumping || currentVerticalState == VState.falling)
            {
                Debug.Log("Jump in air");
                jumpForceValue = (jumpForce + 2) - armourCheck.reduceJumpForce;
                return jumpForceValue;
            }
        }
        return 0;
    }
    public int FacingDirection()
    {
        var _facingDirection = facingDirection;
        return _facingDirection;
    }

    public VState GetVerticalState()
    {
        return currentVerticalState;
    }
    public bool GetBlocking()
    {
        return blocking;
    }
    private void CheckDirection()
    {
        var _facingDirection = playerInput.GetHorizontal();

        if (_facingDirection > 0)
        {
            if (canTurn == false)
            {
                return;
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("Right");
            _facingDirection = 1;
            facingDirection = (int)_facingDirection;
        }
        else if (_facingDirection < 0)
        {
            if (canTurn == false)
            {
                return;
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Left");
            _facingDirection = -1;
            facingDirection = (int)_facingDirection;
        }
    }
    #region Raycast Checker

    public Wall GetCurrentWall()
    {
        return currentWall;
    }
    public void SetCurrentWallNone()
    {
        currentWall = Wall.none;
    }

    public void RaycastGroundCheck(RaycastHit hit)
    {
        if (currentVerticalState == VState.falling)
        {
            if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Platform")))
            {
                Debug.Log("Hit Ground");
                LandOnGround(hit);
            }
            else
            {
                Debug.Log("Not on ground!");
                currentJumpIndex++;
            }
        }
    }
    private void LandOnGround(RaycastHit hit)
    {
        distanceToGround = hit.distance;
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        if (distanceToGround >= 0 && distanceToGround <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.85f, 0);
        }
        distanceToGround = hit.distance;

        currentVerticalState = VState.grounded;
        canDoubleJump = true;
        currentJumpIndex = 0;
    }
    public void PlayerGroundedIsFalse()
    {
        currentVerticalState = VState.falling;
    }
    public void RayCasterLeftWallCheck(RaycastHit hit)
    {
        if (currentVerticalState == VState.jumping || currentVerticalState == VState.falling || currentVerticalState == VState.grounded)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentWall = Wall.leftWall;
                HitLeft(hit);
            }
        }
    }
    public void HitLeft(RaycastHit hit)
    {
        distanceToLeft = hit.distance;
        if (distanceToLeft >= 0 && distanceToLeft <= 0.2f)
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        distanceToLeft = hit.distance;
        currentWall = Wall.leftWall;
    }
    public void RayCasterRightWallCheck(RaycastHit hit)
    {
        if (currentVerticalState == VState.jumping || currentVerticalState == VState.falling || currentVerticalState == VState.grounded)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentWall = Wall.rightWall;
                HitRight(hit);
            }
        }
    }
    public void HitRight(RaycastHit hit)
    {
        distanceToRight = hit.distance;
        if (distanceToRight >= 0 && distanceToRight <= 0.2f)
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        distanceToRight = hit.distance;
        currentWall = Wall.rightWall;
    }

    public void RayCastCeilingCheck(RaycastHit hit)
    {
        if (currentVerticalState == VState.jumping)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                HitCeiling(hit);
            }
        }
    }

    public void HitCeiling(RaycastHit hit)
    {
        distanceToCeiling = hit.distance;
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        if (distanceToCeiling >= 0 && distanceToCeiling <= 0.37f)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.9f, 0);
        }
        distanceToCeiling = hit.distance;
    }
    #endregion

    void Observation()
    {
        V3Velocity = rb.velocity;
        previousVelocity = rb.velocity.y;
    }
}

