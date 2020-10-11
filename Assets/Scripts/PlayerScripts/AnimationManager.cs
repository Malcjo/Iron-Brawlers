using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    Player playerScript;

    public int comboStep;
    public float comboTimer;


    private void Start()
    {
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

    public void Running(bool val)
    {
        anim.SetBool("Running", val);
    }

    public void Idle()
    {
        anim.SetBool("Running", false);
    }
   

    public void Crouching(bool val)
    {
        anim.SetBool("isCrouching", val);
    }

    public void Jump(bool val)
    {
        anim.SetBool("Jumping", val);
    }

    public void LegSweep()
    {
        anim.Play("Leg Sweep");
    }

    public void AerialAttack()
    {
        anim.Play("Aerial Attack");
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
