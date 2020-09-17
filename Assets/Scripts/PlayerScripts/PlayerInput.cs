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

    [Range(-1,1)]
    public int FacingDirection;

    [SerializeField]
    bool running;
    public bool canJump;

    [SerializeField]
    animationGroup state;

    public enum animationGroup { idle, crouching, jumping, attack }

    private void Awake()
    {
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
        JumpInput();
        CrouchCheck();


        if (player.grounded == false)
        {
            state = animationGroup.jumping;
        }

        
    }
    void CrouchCheck()
    {
        if (player.grounded == true)
        {
            if (Input.GetKey(controls.crouchKey))
            {
                state = animationGroup.crouching;

                if (state == animationGroup.crouching)
                {
                    animationScript.Crouching(true);

                    if (Input.GetKeyDown(controls.jabKey))
                    {
                        state = animationGroup.attack;
                        animationScript.LegSweep();
                    }
                }
                if(running == true)
                {
                    animationScript.Crouching(false);
                }
            }
            else
            {
                animationScript.Crouching(false);
            }
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
                animationScript.Running();
                running = true;
                animationScript.Crouching(false);
            }
            if (horizontalInput > 0)
            {
                FacingDirection = 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animationScript.Running();
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
            animationScript.Jump();
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

