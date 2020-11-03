using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerIndex { Player1, Player2 };

    [SerializeField]
    public float gravity = -10f;
    [SerializeField]
    private float friction = 0.25f;
    [SerializeField]
    private int maxLives = 3;

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField] 
    private PlayerInput playerInput;
    [SerializeField] 
    private ArmourCheck armourCheck;
    [SerializeField] 
    private Raycasts raycasts;
    [SerializeField] 
    private PlayerActions playerActions;

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

    public bool DebugModeOn;

    private PlayerIndex playerNumber;
    private int characterType;

    private bool canHitBox;
    private bool inAnimation;
    private bool grounded;
    private bool hasArmour;
    private bool hitStun;
    private bool blocking;
    private bool canJump;
    private bool canAirMove;
    private bool reduceAddForce;
    private bool gravityOn;
    private bool canTurn;
    private bool falling;
    private bool jumping;
    private bool running;
    private float hitStunTimer;

    private int currentJumpIndex;
    private int maxJumps = 2;
    private int facingDirection;

    private Vector3 addForceValue;
    private Vector3 hitDirection;

    private Transform[] characterJoints;

    private string PlayerTextStart;
    private int lives;

    public enum State
    {
        idle,
        crouching,
        jumping,
        running,
        busy
    }

    [SerializeField]
    private State CurrentState;

    void Start()
    {
        gravityOn = true;
        currentJumpIndex = maxJumps;
        blocking = false;
        lives = maxLives;
        canHitBox = true;
        switch (playerNumber)
        {
            case PlayerIndex.Player1: PlayerTextStart = "P1: "; break;
            case PlayerIndex.Player2: PlayerTextStart = "P2: "; break;
        }
        armourCheck.SetAllArmourOn();
    }
    private void Update()
    {
        hasArmour = armourCheck.HasArmour();
        playerLives.text = (PlayerTextStart + lives);
        playerImage.sprite = GameManager.GetSprite(playerNumber);
        ReduceCounter();
        if (GameManager.CheckIsInBounds(transform.position))
        {
            lives--;
            if (lives < 0)
            {
                Die();
            } else
            {
                Respawn();
            }
        }
    }
    void CheckState()
    {
        if (grounded == true)
        {
            currentJumpIndex = 2;
            canTurn = true;
            playerActions.Jump(false);
            playerActions.DoubleJump(false);
            CurrentState = State.idle;

            if (Input.GetKeyDown(controls.jabKey))
            {
                if (blocking == true)
                {
                    return;
                }
                attackManager.Jab();
            }


            if (Input.GetKey(controls.crouchKey))
            {
                if (blocking == true)
                {
                    return;
                }
                CurrentState = State.crouching;
                if (Input.GetKeyDown(controls.jabKey))
                {
                    if (blocking == true)
                    {
                        return;
                    }
                    attackManager.LegSweep();
                }
            }
            if (playerInput.GetHorizontal() > 0 || playerInput.GetHorizontal() < 0)
            {
                CurrentState = State.running;
                Slide();

                if (Input.GetKeyDown(controls.jabKey))
                {
                    attackManager.Heavy();
                }
            }
        }
        else if (grounded == false)
        {
            canTurn = false;
            if (Input.GetKey(controls.jumpKey))
            {
                CurrentState = State.jumping;
                if (blocking == true)
                {
                    return;
                }
            }
        }

        DestroyArmourKnockBack();
        AeiralAttackCheck();
        DoubleJumpCheck();
        switch (CurrentState)
        {
            case State.idle: IdleStateCheck(); break;
            case State.jumping: JumpStateCheck(); break;
            case State.running: RunningStateCheck(); break;
            case State.crouching: CrouchStateCheck(); break;
        }
    }

    private void Attack()
    {
        BusyCheck();
        if (jumping || falling)
        {
            playerActions.AerialAttack();
        }
    }
    void Slide()
    {
        if (Input.GetKeyDown(controls.crouchKey))
        {
            Debug.Log("Slide");
        }
    }

    void IdleStateCheck()
    {
        playerActions.Crouching(false);
        playerActions.Jump(false);
        playerActions.Running(false);
    }

    void RunningStateCheck()
    {
        if (grounded == true)
        {
            playerActions.Running(true);
            if (Input.GetKey(controls.crouchKey))
            {
                playerActions.Crouching(false);
            }
        }
    }

    void JumpStateCheck()
    {
        playerActions.Jump(true);
        canDoubleJump = true;

    }
    void DoubleJumpCheck()
    {
        if (canDoubleJump == true)
        {
            if (falling = true || jumping == true)
            {
                if (Input.GetKeyDown(controls.jumpKey))
                {
                    canTurn = true;
                    playerActions.DoubleJump(true);
                }
            }
        }
        canDoubleJump = false;

    }
    void CrouchStateCheck()
    {
        playerActions.Crouching(true);
    }

    private void Die()
    {
        Debug.Log("Im ded fxi tithis");
    }

    private void Respawn()
    {
        Debug.Log("I'm currently here " + transform.position + " Do something becaus i'm ded");
        transform.position = new Vector3(0, 10, 0);
    }

    private void FixedUpdate()
    {
        CurrentVelocity = rb.velocity.magnitude;
        YVelocity = rb.velocity.y;
        Move();
        if(gravityOn == false)
        {
            return;
        }
        else if(gravityOn == true)
        {
            Gravity();
        }

        if(rb.velocity.y < -20)
        {
            rb.velocity = new Vector3(playerInput.GetHorizontal() * playerStats.CharacterSpeed(), -20, 0) + addForceValue;
        }
    }

    void Move()
    {
        if(CurrentState == State.busy)
        {
            return;
        }
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
                    rb.velocity = new Vector3(playerInput.GetHorizontal() * playerStats.CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
                }
            }
        }
        else
        {
            rb.velocity = new Vector3(playerInput.GetHorizontal() * playerStats.CharacterSpeed(), rb.velocity.y, 0) + addForceValue;
        }
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
            running = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingDirection = 1;
        }
        else if (facingDirection < 0)
        {
            if (canTurn == false)
            {
                return;
            }
            running = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingDirection = -1;
        }
    }
    public int FacingDirection()
    {
        var _facingDirection = facingDirection;
        return _facingDirection;
    }
    void Gravity()
    {
        if (grounded == false)
        {
            rb.AddForce(Vector3.down * gravity * ((playerStats.weight + armourCheck.armourWeight) / 10));
        }
        else if(grounded == true)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            gravity = 0;
        }
    }
    public void Jump()
    {
        if (playerInput.ShouldJump())
        {
            if (currentJumpIndex < maxJumps)
            {
                grounded = false;
                rb.velocity = (new Vector3(rb.velocity.x, playerStats.JumpForceCalculator(), rb.velocity.z));
                currentJumpIndex++;
                playerActions.Jump(true);
            }
        }
    }

    private void ArmourBreak()
    {
        BusyCheck();
        if (hasArmour)
        {
            playerActions.ArmourBreak();
        }
    }
    private void Block()
    {
        BusyCheck();
        if (playerInput.ShouldBlock())
        {
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
        addForceValue = AddForce(Power - (armourCheck.knockBackResistance + playerStats.knockbackResistance));
    }
}

