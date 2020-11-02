using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    Player player;

    public int comboStep;
    public float comboTimer;


    private void Start()
    {
        player = GetComponentInParent<Player>();

        OutAnimation();
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
            player.inAnimation = true;
            anim.Play(animlist[comboStep]);
            comboStep++;
            comboTimer = 1;
        }
    }
    /*
    public void JumpPrep()
    {
        anim.SetBool("Jumping", true);
    }
    */
    public void JumpLanding(bool val)
    {
        anim.SetBool("Jumping", val);
        
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

    public void DoubleJump(bool val)
    {
        anim.SetBool("DoubleJump", val);
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
        player.inAnimation = true;
    }
    public void OutAnimation()
    {
        player.inAnimation = false;
    }

    public void ArmourBreak()
    {
        anim.Play("Armour Break");
    }

    public void HitStun()
    {
        anim.Play("HitStun");
    }

    public void Heavy()
    {
        anim.Play("Heavy");
    }

    public void GravityOff(float pause)
    {
        player.gravityOn = false;
        StartCoroutine(GravityCountDown(pause));

    }
    IEnumerator GravityCountDown(float pause)
    {
        yield return new WaitForSeconds(pause);
        player.gravityOn = true;
    }
}
