using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    public enum PlayerIndex { Player1, Player2, NullPlayer };

    public PlayerIndex playerNumber;
    [SerializeField] private VState _currentVerticalState;
    [SerializeField] private VState _previousVerticalState;
    [SerializeField] private bool standalone;
    [SerializeField] private string CurrentStateName;

    [SerializeField] public float gravityValue = -10f;
    [SerializeField] private float friction = 0.25f;
    [SerializeField] private int maxLives = 3;

    [SerializeField] private float speed = 6.5f;

    [SerializeField] private float weight = 22;
    [SerializeField] private float knockbackResistance = 3;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject hitbox;

    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private ArmourCheck armourCheck;
    [SerializeField] private Raycasts raycasts;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private GaugeManager gaugeManager;

    [SerializeField] private GameObject DoubleJumpDustParticles;
    [SerializeField] private GameObject landOnGroundDustParticle;
    [SerializeField] private GameObject RunningParticle;

    [Header("UI")]
    [SerializeField] public TMP_Text playerLives;
    [SerializeField] public Image playerImage;

    [Header("Observation Values")]
    [SerializeField] private float CurrentVelocity;
    [SerializeField] private float YVelocity;
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

    public bool canDoubleJump;

    [SerializeField] private int _currentJumpIndex;
    private int maxJumps = 2;

    [SerializeField] private Vector3 addForceValue;
    private Vector3 hitDirection;

    private Wall currentWall;
    private PlayerState MyState;

    private bool _wasAttacking;
    private int facingDirection;
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

    [SerializeField] private float attackedFreezeCounter;
    [SerializeField] private float MaxFreezeCounter;
    [SerializeField] private Vector3 _TempAttackedVelocity;
    [SerializeField] private bool freezeAttackedPlayer;

    [SerializeField] private float attackingFreezeCounter;
    [SerializeField] private Vector3 _TempAttackingVelocity;
    [SerializeField] private bool freezeAttackingPlayer;
    private float _PlayerInput;
    private Vector3 _TempDirection;
    private float _tempPower;
    [SerializeField] private bool isDummy;

    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Transform StandaloneSpawnPoint;
    public void SetUpInputDetectionScript(PlayerInputHandler _playerInputDetection)
    {
        playerInputHandler = _playerInputDetection;
    }

    public void SetSpawnPoint(Transform _spawnPoint)
    {
        SpawnPoint = _spawnPoint;
    }
    public int GetPlayerIndex()
    {
        return (int)playerNumber;
    }
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
    void Awake()
    {
        if(standalone == true)
        {
            if(GetPlayerIndex() == 0)
            {
                transform.position = StandaloneSpawnPoint.transform.position;
            }
            else if(GetPlayerIndex()== 1)
            {
                transform.position = StandaloneSpawnPoint.transform.position;
            }

        }
        else
        {

        }

        MyState = new IdleState();
        _gravityOn = true;
        _canTurn = true;
        canAirMove = true;
        _canMove = true;
    }
    private void Start()
    {
        if(playerNumber == PlayerIndex.Player1)
        {
            hitbox.gameObject.layer = 8;
            SpawnPoint = GameManager.instance.player1Spawn;
        }
        else if(playerNumber == PlayerIndex.Player2)
        {
            hitbox.gameObject.layer = 9;
            SpawnPoint = GameManager.instance.player2Spawn;
        }
        transform.position = SpawnPoint.transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            addForceValue = new Vector3(0,10,0);
        }
        CheckDirection();
        CharacterStates();
        ReduceCounter();
        currentPushPower = _currentPushPower;
        //Debug.Log("Horizontal " + playerInput.GetHorizontal());
       // Debug.Log("Vertial " + playerInput.GetVertical());

    }
    private void FixedUpdate()
    {
        Observation();
        GravityCheck();
    }
    #region State Machine
    [SerializeField] float ObservableVerticalCheck;

    public void ViewVertical(float VerticalCheck)
    {
        ObservableVerticalCheck = VerticalCheck;
    }

    private void CharacterStates()
    {
        JumpingOrFallingTracker();
        CurrentStateName = MyState.GiveName();
        if (playerInputHandler == null)
        {
            return;
        }
        MyState.RunState
            (
            this,
            rb,
            playerActions,
            new PlayerState.InputState()
            {
                horizontalInput = playerInputHandler.GetHorizontal(),
                attackInput = playerInputHandler.ShouldAttack(),
                jumpInput = playerInputHandler.ShouldJump(),
                crouchInput = playerInputHandler.ShouldCrouch(),
                armourBreakInput = playerInputHandler.ShouldArmourBreak(),
                blockInput = playerInputHandler.ShouldBlock()
            },
            new PlayerState.Calculating()
            {
                jumpForce = JumpForceCalculator(),
                friction = friction,
                characterSpeed = SetPlayerSpeed(),
                addForce = addForceValue
            }
            );
    }

    public void SetState(PlayerState state)
    {
        MyState = state;
    }
    #endregion

    private IEnumerator StopCharacter()
    {
        yield return new WaitForSeconds(0.1f);
        StopMovingCharacterOnXAxis();
    }
    public void StopMovingCharacterOnXAxis()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    public void StopMovingCharacterOnYAxis()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
    }

    public void Standalone(bool isStandalone)
    {
        standalone = isStandalone;
    }
    public void spawnLandingDustParticles()
    {
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(90, 0, 0);
        GameObject LandingParticles = Instantiate(landOnGroundDustParticle, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(LandingParticles);
    }
    public void SpawnDoubleJumpParticles()
    {
        StartCoroutine(DoubleJumpParticles());
    }
    IEnumerator DoubleJumpParticles()
    {
        yield return new WaitForSeconds(0);
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(90, 0, 0);
        GameObject DoubleJumpParticles = Instantiate(DoubleJumpDustParticles, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(DoubleJumpParticles);
    }
    public void SpawnRunningParticles()
    {
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x - 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(0, 90, 0);
        GameObject RunningParticles = Instantiate(RunningParticle, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(RunningParticles);
    }
    private void SetRemoveParticles(GameObject obj)
    {
        StartCoroutine(RemoveParticles(obj));
    }
    IEnumerator RemoveParticles (GameObject obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj.gameObject);
    }

    #region Jumping
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
    void MinusJumpIndexWhenNotOnGround()
    {
        if(_currentJumpIndex == 0)
        {
            _currentJumpIndex = 1;
        }
    }
    #endregion
    #region Gravity methods
    void TerminalVelocity()
    {
        if (rb.velocity.y < -20)
        {
            if (isDummy == false)
            {
                rb.velocity = new Vector3(playerInputHandler.GetHorizontal() * SetPlayerSpeed(), -20, 0) + addForceValue;

            }
            else
            {
                rb.velocity = addForceValue;
            }

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
        rb.velocity = new Vector3(rb.velocity.x + facingDirection * MoveStrength, rb.velocity.y, 0) * Time.deltaTime;
        rb.AddForce(new Vector3(facingDirection * MoveStrength, rb.velocity.y, 0));
        StartCoroutine(StopCharacter());
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
                if(gravityValue > 10)
                {
                    gravityValue = 10;
                }
                gravityValue = 10;
            }
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + -(gravityValue * ((weight + armourCheck.armourWeight) / 10)) * Time.deltaTime, 0);
        }
        else if (_currentVerticalState == VState.grounded && MyState.StickToGround())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            gravityValue = 0;
        }
    }
    #endregion
    #region ReduceValues
    void ReduceCounter()
    {
        ReduceHitForce();
        ReduceHitStun();
        ReduceAttackedFreezeFrameCounter();
        ReduceAttackingFreezeFrameCounter();
    }
    void ReduceAttackedFreezeFrameCounter()
    {
        attackedFreezeCounter -= 8f * Time.deltaTime;
        if (attackedFreezeCounter <= 0.001f)
        {
            attackedFreezeCounter = 0;
            if (freezeAttackedPlayer == true)
            {
                rb.velocity = _TempAttackedVelocity;
                UseGravity = true;
                freezeAttackedPlayer = false;
                playerActions.ResumeCurrentAnimation();
                Damage(_TempDirection, _tempPower);
            }
        }
    }
    void ReduceAttackingFreezeFrameCounter()
    {
        attackingFreezeCounter -= 8f * Time.deltaTime;
        if(attackingFreezeCounter <= 0.001f)
        {
            attackingFreezeCounter = 0;
            if(freezeAttackingPlayer == true)
            {
                rb.velocity = _TempAttackedVelocity;
                UseGravity = true;
                freezeAttackingPlayer = false;
                playerActions.ResumeCurrentAnimation();
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
    #region Attacking and Damaging
    public void HitStun()
    {
        playerActions.JabHitStun();
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
    public void FreezeCharacterAttacking()
    {
        playerActions.PauseCurrentAnimation();
        freezeAttackingPlayer = true;
        _TempAttackingVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        UseGravity = false;
        attackingFreezeCounter = MaxFreezeCounter;
    }
    public void FreezeCharacterBeingAttacked(Vector3 Direction, float Power)
    {
        playerActions.PauseCurrentAnimation();
        freezeAttackedPlayer = true;
        _TempAttackedVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        UseGravity = false;
        attackedFreezeCounter = MaxFreezeCounter;
        _tempPower = Power;
        _TempDirection = Direction;
    }

    public void TakeDamageOnGauge(float amount, ArmourCheck.ArmourPlacement Placement, AttackType attackType)
    {
        gaugeManager.TakeDamage(amount, Placement, this, attackType);
    }
    public void PlayArmourHitSound(bool Armour, AttackType attackType)
    {
        if (Armour)
        {
            switch (attackType)
            {
                case AttackType.Jab:
                    FindObjectOfType<AudioManager>().Play(AudioManager.JABHITARMOUR);
                    break;

                case AttackType.LegSweep:
                    break;
                case AttackType.Aerial:
                    break;
                case AttackType.ArmourBreak:
                    break;
                case AttackType.HeavyJab:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITARMOUR);
                    break;
            }
        }
        else
        {
            switch (attackType)
            {
                case AttackType.Jab:
                    FindObjectOfType<AudioManager>().Play(AudioManager.JABHITUNARMOURED);
                    break;

                case AttackType.LegSweep:
                    break;
                case AttackType.Aerial:
                    break;
                case AttackType.ArmourBreak:
                    break;
                case AttackType.HeavyJab:
                    FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
                    break;
            }
        }
    }


    #endregion

    public float SendAbsPlayerInputValueToActionsScript()
    {
        return Mathf.Abs(_PlayerInput);
    }

    public void GetPlayerInputFromInputScript(float PlayerInput)
    {
        _PlayerInput = PlayerInput;
    }
    public float SetPlayerSpeed()
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

    #region Direction Player is Facing
    public int GetFacingDirection()
    {
        var _facingDirection = facingDirection;
        return _facingDirection;
    }
    private void CheckDirection()
    {
        if (playerInputHandler == null)
        {
            return;
        }
        var _facingDirection = playerInputHandler.GetHorizontal();
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
    #endregion
    #region Jumping or Falling
    public void JumpingOrFallingAnimations()
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
    void JumpingOrFallingTracker()
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
    #endregion
    #region Raycasts

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
                Invoke("spawnLandingDustParticles", 0.06f);
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
            rb.MovePosition(new Vector3(hit.point.x, hit.point.y, 0));
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
    }
}

