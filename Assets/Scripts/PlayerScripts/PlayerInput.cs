﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerInput : MonoBehaviour
{
    public float horizontal; // Being checked in Player script: MoveCall()
    public int numberOfJumps; //Being checked in Player script: JumpMove()

    float horizontalInput;
    public int maxJumps;

    AnimationManager animationScript;
    Player player;
    PlayerControls controls;
    ArmourCheck armourCheck;
    TempHitBox hitboxScript;
    HitBoxManager hitBoxManager;
    Checker checker;
    AttackManager attackManger;
    [Range(-1, 1)] public int FacingDirection;

    public bool running;
    [SerializeField] private bool jumping;
    [SerializeField] private bool falling;
    
    public bool canJump;
    
    public enum wallCollision {leftWall, rightWall, none}
    public wallCollision wall;

    public enum animationGroup { idle, crouching, jumping, attack, running }
    [SerializeField] animationGroup state;
    private void Awake()
    {
        attackManger = GetComponent<AttackManager>();
        checker = GetComponent<Checker>();
        armourCheck = GetComponent<ArmourCheck>();
        hitBoxManager = GetComponentInChildren<HitBoxManager>();
        hitboxScript = GetComponentInChildren<TempHitBox>();
        player = GetComponent<Player>();
        controls = GetComponent<PlayerControls>();
        animationScript = GetComponentInChildren<AnimationManager>();
    }
    private void Update()
    {
        States();
        Escape();
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
            animationScript.Jump(false);
            state = animationGroup.idle;
            if (Input.GetKeyDown(controls.jabKey))
            {
                attackManger.Jab();
            }

            if (Input.GetKey(controls.crouchKey))
            {
                state = animationGroup.crouching;
                if (Input.GetKeyDown(controls.jabKey))
                {
                    attackManger.LegSweep();
                }
            }
            if (horizontalInput > 0 || horizontalInput < 0)
            {
                state = animationGroup.running;
                Slide();
            }

            if (Input.GetKey(controls.jumpKey))
            {
                state = animationGroup.jumping;
            }
        }
        DestroyArmourKnockBack();
        AeiralAttackCheck();
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
        animationScript.Crouching(false);
        animationScript.Jump(false);
        animationScript.Running(false);
    }

    void RunningStateCheck()
    {
        if (player.grounded == true)
        {
            animationScript.Running(true);
            if (Input.GetKey(controls.crouchKey))
            {
                animationScript.Crouching(false);
            }
        }
    }

    void JumpStateCheck()
    {
        animationScript.Jump(true);
    }

    void CrouchStateCheck()
    {
        animationScript.Crouching(true);
    }
    
    public void HorizontalInput()
    {
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
            animationScript.Crouching(false);
        }
        if (horizontalInput > 0)
        {
            FacingDirection = 1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            running = true;
            animationScript.Crouching(false);
        }
        if (horizontalInput == 0)
        {
            animationScript.Idle();
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
        if (Input.GetKeyDown(controls.jumpKey))
        {
        animationScript.Jump(true);
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
        if (player.jumping == true && Input.GetKeyDown(controls.jabKey))
        {
            attackManger.AerialAttack();
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
        if (Input.GetKey(controls.armourKey) && Input.GetKey(controls.crouchKey))
        {
            if(player.hasArmour == false)
            {
                return;
            }
            else if(player.hasArmour == true)
            {
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
            animationScript.JumpLanding();
            numberOfJumps = maxJumps;
        }
    }
    void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
 }   

