using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerInput : MonoBehaviour
{
    public float horizontal; // Being checked in Player script: MoveCall()
    public int numberOfJumps; //Being checked in Player script: JumpMove()

    [SerializeField] private float horizontalInput;
    [SerializeField] public int maxJumps;
    AnimationManager animationScript;

    [SerializeField] private Player player;
    [SerializeField] private PlayerControls controls;
    [SerializeField] private TempHitBox hitboxScript;
    [SerializeField] private HitBoxManager hitBoxManager;

    [Range(-1,1)]
    public int FacingDirection;

    [SerializeField]
    bool running;
    [SerializeField]
    bool jumping;
    public bool canJump;

    [SerializeField]
    animationGroup state;

    public enum animationGroup { idle, crouching, jumping, attack, running }

    private void Awake()
    {
        hitBoxManager = GetComponentInChildren<HitBoxManager>();
        hitboxScript = GetComponentInChildren<TempHitBox>();
        player = GetComponent<Player>();
        controls = GetComponent<PlayerControls>();
        animationScript = GetComponentInChildren<AnimationManager>();
    }
    private void Update()
    {
        Escape();
        HorizontalInput();
        AttackInput();
        BlockInput();
        JumpInput();
        CheckState(); 
    }

    void CheckState()
    {
        if (player.grounded == true)
        {
            animationScript.Jump(false);
            state = animationGroup.idle;

            if (Input.GetKey(controls.crouchKey))
            {
                state = animationGroup.crouching;
            }
            if (horizontalInput > 0 || horizontalInput < 0)
            {
                state = animationGroup.running;
            }
            if (Input.GetKey(controls.jumpKey))
            {
                state = animationGroup.jumping;
            }
        }
        switch (state)
        {
            case animationGroup.idle: IdleStateCheck(); break;
            case animationGroup.jumping: JumpStateCheck(); break;
            case animationGroup.running: RunningStateCheck(); break;
            case animationGroup.crouching: CrouchStateCheck(); break;
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

        if (Input.GetKeyDown(controls.jabKey))
        {
            
            hitboxScript._attackType = TempHitBox.AttackType.Aerial;
            hitboxScript._attackDir = TempHitBox.Attackdirection.Aerial;
            state = animationGroup.attack;
            animationScript.AerialAttack();
            player.inAnimation = false;
        }
    }

    void CrouchStateCheck()
    {
        animationScript.Crouching(true);

        if (Input.GetKeyDown(controls.jabKey))
        {
            hitboxScript._attackDir = TempHitBox.Attackdirection.Low;
            hitboxScript._attackType = TempHitBox.AttackType.LegSweep;
            state = animationGroup.attack;
            animationScript.LegSweep();
        }
    }
    
    public void HorizontalInput()
    {

        horizontalInput = Input.GetAxisRaw(controls.horizontalKeys);
        horizontal = (horizontalInput);

       
        if (player.inAnimation == true)
        {
            return;
        }
        
        if (horizontalInput < 0)
        {
            FacingDirection = -1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //animationScript.Running();
            running = true;
            animationScript.Crouching(false);
        }
        if (horizontalInput > 0)
        {
            FacingDirection = 1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //animationScript.Running();
            running = true;
            animationScript.Crouching(false);
        }
        if (horizontalInput == 0)
        {

            animationScript.Idle();
            running = false;
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

    public void AttackInput()
    {
        if (Input.GetKeyDown(controls.jabKey))
        {
            hitboxScript._attackDir = TempHitBox.Attackdirection.Forward;
            hitboxScript._attackType = TempHitBox.AttackType.Jab;
            animationScript.JabCombo();
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

