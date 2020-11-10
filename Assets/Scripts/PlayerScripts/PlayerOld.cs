using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOld : MonoBehaviour
{
    public enum PlayerIndex { Player1, Player2 };

    public PlayerIndex playerNumber;

    public State CurrentState;
    public VState currentVerticalState;


    [SerializeField]
    public float gravity = -10f;
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

    public bool DebugModeOn;



    private bool canHitBox;
    private bool hasArmour;
    private bool hitStun;
    private bool blocking;
    [SerializeField] private bool canJump;
    private bool canAirMove;
    private bool reduceAddForce;
    [SerializeField]
    private bool gravityOn;
    private bool canTurn;
    private bool canDoubleJump;

    [SerializeField] private float jumpForce = 9;
    private float hitStunTimer;


    [SerializeField] private int currentJumpIndex;
    [SerializeField] private int maxJumps = 2;
    private int facingDirection;

    private Vector3 addForceValue;
    private Vector3 hitDirection;
    public int lives;
    public int characterType;

    private Wall currentWall;
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
    public enum State
    {
        idle,
        crouching,
        moving,
        busy
    }


    void Start()
    {
        gravityOn = true;
        currentJumpIndex = maxJumps;
        blocking = false;
        lives = maxLives;
        canHitBox = true;

        armourCheck.SetAllArmourOn();
    }
    private void Update()
    {
        CheckState();
        Block();
        ArmourBreak();
        CheckDirection();
        hasArmour = armourCheck.HasArmour();
        ReduceCounter();


        playerLives.text = (playerSetup.GetPlayerText() + lives);

        if (GameManager.CheckIsInBounds(transform.position))
        {
            lives--;
            if (lives < 0)
            {
                Die();
            }
            else
            {
                Respawn();
            }
        }
    }
    private void FixedUpdate()
    {
        VerticalState();
        Move();

        CurrentVelocity = rb.velocity.magnitude;
        YVelocity = rb.velocity.y;


        if (gravityOn == false)
        {
            return;
        }
        else if (gravityOn == true)
        {
            Gravity();
        }

        if (rb.velocity.y < -20)
        {
            rb.velocity = new Vector3(playerInput.GetHorizontal() * CharacterSpeed(), -20, 0) + addForceValue;
        }
    }
    void CheckState()
    {
        if (currentVerticalState == VState.grounded)
        {
            currentJumpIndex = 2;
            canTurn = true;
            playerActions.Jump(false);
            playerActions.DoubleJump(false);
            CurrentState = State.idle;
            if (playerInput.ShouldAttack() == true)
            {
                if (CurrentState == State.idle)
                {
                    CurrentState = State.busy;
                    playerActions.JabCombo();
                    Debug.Log("Jab");
                }
                else
                {
                    if (CurrentState == State.crouching)
                    {
                        CurrentState = State.busy;
                        playerActions.LegSweep();
                        Debug.Log("Leg Sweep");
                    }
                    else
                    {
                        if (CurrentState == State.moving)
                        {
                            CurrentState = State.busy;
                            playerActions.Heavy();
                            Debug.Log("Heavy");
                        }
                    }
                }
            }

        }
        else if (currentVerticalState == VState.jumping || currentVerticalState == VState.falling)
        {
            canTurn = false;
            if (playerInput.ShouldAttack() == true)
            {
                CurrentState = State.busy;
                playerActions.AerialAttack();
                Debug.Log("Aerial");
            }
        }
    }
    public bool GetBlocking()
    {
        return blocking;
    }

    public void Jump()
    {
        if (playerInput.ShouldJump())
        {
            if (currentJumpIndex < maxJumps)
            {
                DoubleJumpCheck();
                currentVerticalState = VState.jumping;
                rb.velocity = (new Vector3(rb.velocity.x, JumpForceCalculator(), rb.velocity.z));
                currentJumpIndex++;
                playerActions.Jump(true);
            }
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

    public float JumpForceCalculator()
    {
        float jumpForceValue;
        if (currentJumpIndex == 0)
        {
            return SetVelocityY();
        }
        else if (currentJumpIndex > 0)
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
    private void Die()
    {
        Debug.Log("Player Dead");
    }
    private void Respawn()
    {
        Debug.Log("Player current position " + transform.position + "Respawn character");
        transform.position = new Vector3(0, 10, 0);
    }



    void VerticalState()
    {
        previousVelocity = rb.velocity.y;
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
            else if (rb.velocity.y > -0.1f && rb.velocity.y < 0.1f)
            {
                currentVerticalState = VState.grounded;
            }
        }
    }

    void Move()
    {
        if (CurrentState == State.busy)
        {
            return;
        }
        if (CurrentState == State.idle)
        {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y, 0);
            playerActions.Running(false);
            playerActions.Idle(true);
            return;
        }
        else if (CurrentState == State.moving)
        {
            if (currentVerticalState == VState.grounded)
            {
                playerActions.Running(true);
                playerActions.Idle(false);
                rb.velocity = new Vector3(playerInput.GetHorizontal() * CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
            }
            else if (currentVerticalState == VState.jumping || currentVerticalState == VState.falling)
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
        }
    }
    public float CharacterSpeed()
    {
        float characterSpeed = speed - armourCheck.armourReduceSpeed;
        if (hitStun == true)
        {
            characterSpeed *= 0 + (5 * Time.deltaTime);
        }
        return characterSpeed;
    }
    private void CheckDirection()
    {
        var facingDirection = playerInput.GetHorizontal();

        if (facingDirection > 0)
        {
            if (canTurn == false)
            {
                return;
            }
            CurrentState = State.moving;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingDirection = 1;
            if (facingDirection > 0.5f)
            {
                CurrentState = State.moving;

            }
        }
        else if (facingDirection < 0)
        {
            if (canTurn == false)
            {
                return;
            }
            CurrentState = State.moving;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingDirection = -1;
            if (facingDirection < -0.5f)
            {
                CurrentState = State.moving;
            }
        }
        else if (facingDirection == 0)
        {
            CurrentState = State.idle;
        }
    }
    public int FacingDirection()
    {
        var _facingDirection = facingDirection;
        return _facingDirection;
    }
    void Gravity()
    {
        if (currentVerticalState == VState.falling || currentVerticalState == VState.jumping)
        {
            gravity = 10;
            rb.AddForce(Vector3.down * gravity * ((weight + armourCheck.armourWeight) / 10));
        }
        else if (currentVerticalState == VState.grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            gravity = 0;
        }
    }

    private void ArmourBreak()
    {
        BusyCheck();
        if (playerInput.ShouldArmourBreak())
        {
            if (hasArmour)
            {
                playerActions.ArmourBreak();
            }
        }
    }
    private void Block()
    {
        BusyCheck();
        if (playerInput.ShouldBlock())
        {
            CurrentState = State.busy;
            playerActions.Block();
        }
        else
        {
            playerActions.StopBlock();
        }
    }

    private void BusyCheck()
    {
        if (CurrentState == State.busy)
        {
            return;
        }
    }
    public float SetVelocityY()
    {
        return rb.velocity.y;
    }
    public void HitStun()
    {
        playerActions.HitStun();
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
            hitStunTimer -= 1 * Time.deltaTime;
            if (hitStunTimer < 0.001f)
            {
                hitStunTimer = 0;
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
    public void Damage(Vector3 Hit, float Power)
    {
        Debug.Log("Jab");
        hitStun = true;
        hitStunTimer = 1.1f;
        hitDirection = Hit;
        addForceValue = AddForce(Power - (armourCheck.knockBackResistance + knockbackResistance));
    }

    #region Raycast Checker
    private float distanceToGround;
    private float distanceToCeiling;
    private float distanceToRight;
    private float distanceToLeft;
    public void RaycastGroundCheck(RaycastHit hit)
    {
        if (currentVerticalState == VState.falling)
        {
            if (hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Platform")))
            {
                Debug.Log("Hit Ground");
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

        currentVerticalState = VState.grounded;
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


    public void HitLeft(RaycastHit hit)
    {
        distanceToLeft = hit.distance;
        if (distanceToLeft >= 0 && distanceToLeft <= 0.2f)
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        distanceToLeft = hit.distance;
        currentWall = Wall.leftWall;
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
    public Wall GetCurrentWall()
    {
        return currentWall;
    }
    public void SetCurrentWallNone()
    {
        currentWall = Wall.none;
    }
    #endregion

}

