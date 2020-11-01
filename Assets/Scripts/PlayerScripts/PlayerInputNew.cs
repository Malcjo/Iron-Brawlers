using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerInputNew : MonoBehaviour
{
    public float horizontal; // Being checked in Player script: MoveCall()
    public int numberOfJumps; //Being checked in Player script: JumpMove()

    float horizontalInput;
    public int maxJumps;

    AnimationManagerNew animationScript;
    Player player;
    PlayerControls controls;
    ArmourCheck armourCheck;
    Hitbox hitboxScript;
    HitBoxManager hitBoxManager;
    Checker checker;
    AttackManager attackManager;
    [Range(-1, 1)] public int FacingDirection;

    public bool running;
    [SerializeField] private bool jumping;
    [SerializeField] private bool falling;
    [SerializeField] private bool canDoubleJump;
    
    public bool canJump;
    
    public enum wallCollision {leftWall, rightWall, none}
    public wallCollision wall;

    public enum animationGroup { idle, crouching, jumping, attack, running }
    [SerializeField] animationGroup state;
    private void Awake()
    {
        canDoubleJump = false;
        attackManager = GetComponent<AttackManager>();
        checker = GetComponent<Checker>();
        armourCheck = GetComponent<ArmourCheck>();
        hitBoxManager = GetComponentInChildren<HitBoxManager>();
        hitboxScript = GetComponentInChildren<Hitbox>();
        player = GetComponent<Player>();
        controls = GetComponent<PlayerControls>();
        animationScript = GetComponentInChildren<AnimationManagerNew>();
    }
    private void Update()
    {
        States();
        //Escape();
        HorizontalInput();
        //AeiralAttackCheck();
        BlockInput();
        JumpInput();
        CheckState(); 
    }
    void States()
    {
        falling = checker.falling;
        jumping = checker.jumping;
    }
    void CheckState()
    {
        if (player.grounded == true)
        {
            state = animationGroup.idle;

            if (Input.GetKeyDown(controls.jabKey))
            {
                if (player.blocking == true)
                {
                    return;
                }
                attackManager.Jab();
            }


            if (Input.GetKey(controls.crouchKey))
            {
                if (player.blocking == true)
                {
                    return;
                }
                state = animationGroup.crouching;
                if (Input.GetKeyDown(controls.jabKey))
                {
                    if (player.blocking == true)
                    {
                        return;
                    }
                    attackManager.LegSweep();
                }
            }
            if (horizontalInput > 0 || horizontalInput < 0)
            {
                state = animationGroup.running;
                Slide();

                if (Input.GetKeyDown(controls.jabKey))
                {
                    attackManager.Heavy();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(controls.jumpKey))
            {
                state = animationGroup.jumping;
                if (player.blocking == true)
                {
                    return;
                }
            }
        }

        DestroyArmourKnockBack();
        //AeiralAttackCheck();
        //DoubleJumpCheck();
        switch (state)
        {
            case animationGroup.idle: IdleStateCheck(); break;
            case animationGroup.jumping: JumpStateCheck(); break;
            case animationGroup.running: RunningStateCheck(); break;
            case animationGroup.crouching: CrouchStateCheck(); break;
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
        animationScript.Idle();
    }

    void RunningStateCheck()
    {
        if (player.grounded == true)
        {
            animationScript.Running();
            if (Input.GetKey(controls.crouchKey))
            {
                animationScript.Crouching();
            }
        }
    }

    void JumpStateCheck()
    {
        animationScript.Jump();
        canDoubleJump = true;
        
    }
    void DoubleJumpCheck()
    {
        if(canDoubleJump == true)
        {
            if (falling = true || jumping == true)
            {
                if (Input.GetKeyDown(controls.jumpKey))
                {
                    animationScript.DoubleJump();
                }
            }
        }
        canDoubleJump = false;

    }
    void CrouchStateCheck()
    {
        animationScript.Crouching();
    }
    
    public void HorizontalInput()
    {
        if (player.blocking == true)
        {
            horizontalInput = 0;
            horizontal = 0;
            return;
        }
        horizontalInput = Input.GetAxisRaw(controls.horizontalKeys);

        WallCheck();


        horizontal = (horizontalInput);
        if (player.inAnimation == true)
        {
            return;
        }


        if (horizontalInput < 0)
        {
            FacingDirection = -1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            running = true;
           
        }
        if (horizontalInput > 0)
        {
            FacingDirection = 1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            running = true;
           
        }
        if (horizontalInput == 0)
        {
           
            running = false;
        }
    }
    void WallCheck()
    {
        switch (wall)
        {
            case wallCollision.none:
                break;
            case wallCollision.leftWall:
                if (horizontalInput < 0)
                {
                    horizontalInput = 0;
                }
                break;
            case wallCollision.rightWall:
                if (horizontalInput > 0)
                {
                    horizontalInput = 0;
                }
                break;
        }
    }
    public void JumpInput()
    {
        if (player.blocking == true)
        {
            return;
        }
        if (Input.GetKeyDown(controls.jumpKey))
        {
        //animationScript.Jump();
        player.inAnimation = false;
            player.Jump();
            numberOfJumps --;
            if (numberOfJumps < 0)
            {
                numberOfJumps = 0;
            }
        }
        if (numberOfJumps < maxJumps && player.grounded == true)
        {
            numberOfJumps = maxJumps;
        }
    }

    public void AeiralAttackCheck()
    {
        if (player.blocking == true)
        {
            return;
        }
        if ((checker.jumping == true || checker.falling) && Input.GetKeyDown(controls.jabKey))
        {
            attackManager.AerialAttack();
        }
    }
    public void BlockInput()
    {
        if (Input.GetKey(controls.blockKey))
        {
            hitBoxManager.Block();
            player.blocking = true;
        }
        if (Input.GetKeyUp(controls.blockKey))
        {
            hitBoxManager.StopBlock();
            player.blocking = false;
        }
    }
    void DestroyArmourKnockBack()
    {
        if (player.blocking == true)
        {
            return;
        }
        if (Input.GetKey(controls.armourKey) && Input.GetKey(controls.crouchKey))
            
        {
            if(player.hasArmour == false)
            {
                return;
            }
            else if(player.hasArmour == true)
            {
                attackManager.ArmourBreak();
                armourCheck.SetAllArmourOff();
                hitboxScript._attackDir = Attackdirection.Down;
                hitboxScript._attackType = AttackType.Shine;
                hitBoxManager.ShineAttack();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")

        {
            if (!Input.GetKeyDown(controls.jumpKey))
            {
                numberOfJumps++;
            }
            numberOfJumps--;
        }
    }

    //resetting number of jumps to max jumps
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            
            numberOfJumps = maxJumps;
        }
    }
    //void Escape()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Application.Quit();
    //    }
    //}
 }   

