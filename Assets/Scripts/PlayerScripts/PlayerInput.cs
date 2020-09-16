using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerInput : MonoBehaviour
{






    public float horizontal; // Being checked in Player script: MoveCall()
    public int canJump; //Being checked in Player script: JumpMove()

    [SerializeField] private float horizontalInput;
    [SerializeField] public int maxJumps;
    AnimationManager animationScript;




    [SerializeField] private Player player;
    [SerializeField] private PlayerControls controls;
    [SerializeField] private TempHitBox hitboxScript;

    [Range(-1,1)]
    public int FacingDirection;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (player.grounded == true)
        {
            state = animationGroup.idle;
            player.inAnimation = false;

            if (Input.GetKey(controls.crouchKey))
            {
                state = animationGroup.crouching;

                if (state == animationGroup.crouching)
                {
                    player.inAnimation = true;
                    animationScript.Crouching(true);

                    if (Input.GetKeyDown(controls.jabKey))
                    {
                        state = animationGroup.attack;
                        animationScript.LegSweep();
                    }
                }
            }
            else
            {
                animationScript.Crouching(false);
            }
        }

        if (player.grounded == false)
        {
            state = animationGroup.jumping;
        }

        if (Input.GetKey(controls.jumpKey))
        {
            animationScript.Jump();
        }  
    }

        private void FixedUpdate()
        {
            HorizontalInput();
            AttackInput();
            JumpInput();
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
            }
            if (horizontalInput > 0)
            {
                FacingDirection = 1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animationScript.Running();
            }
            if (horizontalInput == 0)
            {

                animationScript.Idle();
            }
        }

        public void JumpInput()
        {
            if (Input.GetKeyDown(controls.jumpKey))
            {
                player.inAnimation = false;
                player.JumpMove();
                canJump -= 1;
                if (canJump < 0)
                {
                    canJump = 0;
                }
            }
            if (canJump > maxJumps && player.grounded == true)
            {
                canJump = maxJumps;
            }
        }

        public void AttackInput()
        {
            if (Input.GetKeyDown(controls.jabKey))
            {
                animationScript.JabCombo();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Ground")

            {
                if (!Input.GetKeyDown(controls.jumpKey))
                {
                    canJump++;
                }
                canJump--;
            }
        }

        //resetting number of jumps to max jumps
        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.tag == "Ground")
            {
                animationScript.JumpLanding();
                canJump = maxJumps;
            }
        }
 }   

