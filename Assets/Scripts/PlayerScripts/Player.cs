using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerIndex { Player1, Player2 };

    public PlayerIndex playerNumber;
    [SerializeField] private VState _currentVerticalState;
    [SerializeField] private VState _previousVerticalState;

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

    [SerializeField] private Rigidbody rb;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ArmourCheck armourCheck;
    [SerializeField] private Raycasts raycasts;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private PlayerSetup playerSetup;

    [Header("UI")]
    [SerializeField]
    public TMP_Text playerLives;
    [SerializeField]
    public Image playerImage;

    [Header("Observation Values")]
    [SerializeField] private float CurrentVelocity;
    [SerializeField] private float YVelocity;
    private float previousVelocity;
    [SerializeField] private Vector3 V3Velocity;
    public bool DebugModeOn;

    private float distanceToGround;
    private float distanceToCeiling;
    private float distanceToRight;
    private float distanceToLeft;
    [SerializeField] private float jumpForce = 9;
    private float hitStunTimer;

    private bool canHitBox;
    private bool hasArmour;
    [SerializeField] private bool hitStun;

    private bool _blocking;
    private bool _canTurn;
    private bool _canMove;


    private bool canJump;
    private bool canAirMove;
    private bool reduceAddForce;

    [SerializeField] private bool _gravityOn;

    private bool canDoubleJump;

    private int _currentJumpIndex;
    private int maxJumps = 2;

    [SerializeField] private Vector3 addForceValue;
    private Vector3 hitDirection;

    private Wall currentWall;
    private PlayerState MyState;

    private bool _wasAttacking;
    public int facingDirection;
    public int lives;
    public int characterType;
    private bool _inAir;
    public bool WasAttacking { get { return _wasAttacking; } set { _wasAttacking = value; } }
    public bool UseGravity { get { return _gravityOn; } set { _gravityOn = value; } }
    public bool InAir { get { return _inAir; } set { _inAir = value; } }
    public bool CanTurn { get { return _canTurn; } set { _canTurn = value; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public int CanJumpIndex { get { return _currentJumpIndex; } set { _currentJumpIndex = value; } }
    public bool GetCanAirMove() { return canAirMove; }
    public bool GetBlocking() { return _blocking; }
    public int GetMaxJumps() { return maxJumps; }
    public VState VerticalState { get { return _currentVerticalState; } set { _currentVerticalState = value; } }
    public VState PreviousVerticalState { get { return _previousVerticalState; } set { _previousVerticalState = value; } }

    [SerializeField] private float freezeCounter;
    [SerializeField] private float maxFreezeCounter;
    [SerializeField] private Vector3 _TempVelocity;
    [SerializeField] private bool freezePlayer;

    private Vector3 _TempDirection;
    private float _tempPower;

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
        _gravityOn = true;
        _canTurn = true;
        canAirMove = true;
        _canMove = true;
    }
    private void Update()
    {
        CheckDirection();
        CharacterStates();
        ReduceCounter();
        currentPushPower = _currentPushPower;

    }
    private void FixedUpdate()
    {
        Observation();
        GravityCheck();
    }
    #region State Machine
    #region Movement

    private void CharacterStates()
    {
        VerticalStateTracker();
        CurrentStateName = MyState.GiveName();
        MyState.RunState
            (
            this,
            rb,
            playerActions,
            new PlayerState.InputState()
            {
                horizontalInput = playerInput.GetHorizontal(),
                attackInput = playerInput.ShouldAttack(),
                jumpInput = playerInput.ShouldJump(),
                crouchInput = playerInput.ShouldCrouch(),
                armourBreakInput = playerInput.ShouldArmourBreak(),
                blockInput = playerInput.ShouldBlock()
            },
            new PlayerState.Calculating()
            {
                jumpForce = JumpForceCalculator(),
                friction = friction,
                characterSpeed = CharacterSpeed(),
                addForce = addForceValue
            }
            );
    }

    public void SetState(PlayerState state)
    {
        MyState = state;
    }

    void DoubleJumpCheck()
    {
        if (canDoubleJump == true)
        {
            _canTurn = true;
            playerActions.DoubleJump(true);
            canDoubleJump = false;
        }
    }

    public void AddOneToJumpIndex()
    {
        _currentJumpIndex++;
    }
    #endregion

    public void StopMovingCharacterOnXAxis()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    public void StopMovingCharacterOnYAxis()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
    }
    #endregion

    public void FreezeCharacterBeingAttacked(Vector3 Direction, float Power)
    {
        playerActions.PauseCurrentAnimation();
        freezePlayer = true;
        _TempVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        UseGravity = false;
        freezeCounter = maxFreezeCounter;
        _tempPower = Power;
        _TempDirection = Direction;
    }
    public void FreezeCharacterAttacking()
    {
        playerActions.PauseCurrentAnimation();
        freezePlayer = true;
        _TempVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        UseGravity = false;
        freezeCounter = maxFreezeCounter;
    }

    void MinusJumpIndexWhenNotOnGround()
    {
        if(_currentJumpIndex == 0)
        {
            _currentJumpIndex = 1;
        }
    }
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
        if (_gravityOn == false)
        {
            return;
        }
        else if (_gravityOn == true)
        {
            Gravity();
        }
    }
    [SerializeField] private float currentPushPower;
    [SerializeField] private float _currentPushPower;
    public void MoveCharacterWithAttacks(float MoveStrength)
    {
        rb.AddForce(new Vector3(facingDirection * (MoveStrength), rb.velocity.y, 0));
    }
    void Gravity()
    {
        TerminalVelocity();

        if (_currentVerticalState != VState.grounded)
        {
            if (_currentVerticalState == VState.falling) 
            {
                gravityValue = 10;
            }
            else if (_currentVerticalState == VState.jumping)
            {
                if(gravityValue == 12)
                {
                    gravityValue = 10;
                }
                gravityValue = 10;
            }
            rb.AddForce(Vector3.down * gravityValue * ((weight + armourCheck.armourWeight) / 10));
        }
        else if (_currentVerticalState == VState.grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            gravityValue = 0;
        }
    }
    #endregion
    #region ReduceValues
    void ReduceCounter()
    {
        ReduceHitForce();
        ReduceHitStun();
        ReduceFreezeFrameCounter();
    }
    void ReduceFreezeFrameCounter()
    {
        freezeCounter -= 8f * Time.deltaTime;
        if (freezeCounter <= 0.001f)
        {
            freezeCounter = 0;
            if (freezePlayer == true)
            {
                rb.velocity = _TempVelocity;
                UseGravity = true;
                freezePlayer = false;
                playerActions.ResumeCurrentAnimation();
                Damage(_TempDirection, _tempPower);
            }
        }
    }
    void ReduceHitForce()
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
            hitStunTimer -= 1 * Time.deltaTime;
            if (hitStunTimer < 0.001f)
            {
                hitStunTimer = 0;
                hitStun = false;
            }
        }
    }
    #endregion
    #region Damaging
    public void HitStun()
    {
        playerActions.HitStun();

    }
    public void Damage(Vector3 Hit, float Power)
    {
        hitStun = true;
        hitStunTimer = 1.1f;
        addForceValue = AddForce(Hit, Power /*- (armourCheck.knockBackResistance + knockbackResistance)*/);
    }
    private Vector3 AddForce(Vector3 HitDirection, float hitStrength)
    {
        Vector3 addForceValue = ((HitDirection) * (hitStrength));
        return addForceValue;
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
        if (_currentJumpIndex == maxJumps)
        {
            return rb.velocity.y;
        }
        else if (_currentJumpIndex < maxJumps)
        {
            if (_currentVerticalState == VState.grounded)
            {
                jumpForceValue = jumpForce - armourCheck.reduceJumpForce;
                return jumpForceValue;
            }
            else if (_currentVerticalState == VState.jumping || _currentVerticalState == VState.falling)
            {
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
    private void CheckDirection()
    {
        var _facingDirection = playerInput.GetHorizontal();

        if (_facingDirection > 0)
        {
            if (_canTurn == false)
            {
                return;
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingDirection = (int)_facingDirection;
        }
        else if (_facingDirection < 0)
        {
            if (_canTurn == false)
            {
                return;
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingDirection = (int)_facingDirection;
        }
    }

    public void CheckVerticalState()
    {
        switch (_currentVerticalState)
        {
            case VState.falling:
                playerActions.Falling();
                break;
            case VState.jumping:
                playerActions.Jumping();
                break;
        }
    }
    void VerticalStateTracker()
    {
        if (rb.velocity.y != 0)
        {
            if (rb.velocity.y > 0.1f)
            {
                _currentVerticalState = VState.jumping;
            }
            else if (rb.velocity.y < -0.1f)
            {
                _currentVerticalState = VState.falling;
            }
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
        if (_currentVerticalState == VState.falling)
        {
            if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Platform")))
            {
                LandOnGround(hit);
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
        _currentVerticalState = VState.grounded;

        canDoubleJump = true;
        _currentJumpIndex = 0;
    }
    public void PlayerGroundedIsFalse()
    {
        _currentVerticalState = VState.falling;
    }
    public void RayCasterLeftWallCheck(RaycastHit hit)
    {
        if (_currentVerticalState == VState.jumping || _currentVerticalState == VState.falling || _currentVerticalState == VState.grounded)
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
        if (_currentVerticalState == VState.jumping || _currentVerticalState == VState.falling || _currentVerticalState == VState.grounded)
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
        if (_currentVerticalState == VState.jumping)
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

