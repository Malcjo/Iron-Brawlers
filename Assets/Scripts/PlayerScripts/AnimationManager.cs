using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    PlayerControls controls;
    Player playerScript;
    PlayerInput playerInput;

    [SerializeField]
    bool canDoubleJump;
    [SerializeField]
    bool jumpButtonPressed;

    public int comboStep;
    public float comboTimer;
    public animationGroup state;

    public enum animationGroup {idle, crouching, jumping}

    private void Start()
    {
        controls = GetComponentInParent<PlayerControls>();
        playerScript = GetComponentInParent<Player>();
        playerInput = GetComponentInParent<PlayerInput>();

        canDoubleJump = false;
        jumpButtonPressed = false;
    }
    private void Update()
    {
        playerScript.inAnimation = Input.GetKey(controls.crouchKey);
        anim.SetBool("isCrouching", Input.GetKey(controls.crouchKey));
        anim.SetBool("Jumping", Input.GetKeyDown(controls.jumpKey));

        if (Input.GetKeyDown(controls.jumpKey))
        {
            jumpButtonPressed = true;
        }

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer < 0)
            {
                comboStep = 0;
            }
        }

        if (playerScript.grounded == true)
        {
            state = animationGroup.idle;
        }

        if (playerScript.grounded == false)
        {
            state = animationGroup.jumping;
        }

        if (playerInput.canJump == 1)
        {
            canDoubleJump = true;
        }

        else
        {
            canDoubleJump = false;
        }

        if (state == animationGroup.jumping && jumpButtonPressed == true && canDoubleJump == true)
        {
            anim.Play("DoubleJump");
            jumpButtonPressed = false;
        }

        if (Input.GetKey(controls.crouchKey))
        {
            state = animationGroup.crouching;
        }
        
        if (state == animationGroup.crouching && Input.GetKeyDown(controls.jabKey))
        {
            playerScript.inAnimation = true;
            anim.Play("Leg Sweep");
        }
    }

    public void JabCombo()
    {
        if (comboStep < animlist.Count)
        {
            playerScript.inAnimation = true;
            anim.Play(animlist[comboStep]);
            comboStep++;
            comboTimer = 1;
        }
    }
    
    public void JumpPrep()
    {
        anim.SetBool("Jumping", true);
    }

    public void JumpLanding()
    {
        anim.SetBool("Jumping", false);
        //anim.SetBool("canDoubleJump", false);
        //canDoubleJump = false;
    }


    public void Running()
    {
        anim.SetBool("Running", true);
    }

    public void Idle()
    {
        anim.SetBool("Running", false);
    }
    public void InAnimation()
    {
        playerScript.inAnimation = true;
    }
    public void OutAnimation()
    {
        playerScript.inAnimation = false;
    }

}
