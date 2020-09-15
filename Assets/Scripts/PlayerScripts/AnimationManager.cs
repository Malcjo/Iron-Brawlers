using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    PlayerControls controls;
    Player playerScript;

    public int comboStep;
    public float comboTimer;


    private void Start()
    {
        controls = GetComponentInParent<PlayerControls>();
        playerScript = GetComponentInParent<Player>();
       
    }
    private void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer < 0)
            {
                comboStep = 0;
            }
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
   

    public void Crouching(bool val)
    {
        anim.SetBool("isCrouching", val);
    }

    public void Jump()
    {
        anim.SetBool("Jumping", true);
    }

    public void LegSweep()
    {
        anim.Play("Leg Sweep");
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
