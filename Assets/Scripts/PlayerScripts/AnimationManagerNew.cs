using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManagerNew : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    Player player;

    const string IDLE = ("Idle");
    const string JUMP = ("Jump");
    const string DOUBLEJUMP = ("Double Jump");
    const string LEGSWEEP = ("Leg Sweep");
    const string AERIAL = ("Aerial Attack");
    const string ARMOURBREAK = ("Armour Break");
    const string HITSTUN = ("HitStun");
    const string HEAVY = ("Heavy");
    const string RUN = ("Fast Run");
    const string CROUCH = ("Crouch Idle");

    public int comboStep;
    public float comboTimer;


    private void Start()
    {
        player = GetComponentInParent<Player>();
       
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
        anim.Play(JUMP);
        
        //anim.SetBool("canDoubleJump", false);
        //canDoubleJump = false;
    }
    
    public void Running()
    {
        anim.Play(RUN);
    }

    public void Idle()
    {
        anim.Play(IDLE);
    }
   

    public void Crouching()
    {
        anim.Play(CROUCH);
    }

    public void Jump()
    {  
        anim.Play(JUMP);
    }

    public void DoubleJump()
    {
        anim.Play(DOUBLEJUMP);
    }

    public void LegSweep()
    {
        anim.Play(LEGSWEEP);
    }

    public void AerialAttack()
    {
        anim.Play(AERIAL);
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
        anim.Play(ARMOURBREAK);
    }

    public void HitStun()
    {
        anim.Play(HITSTUN);
    }

    public void Heavy()
    {
        anim.Play(HEAVY);
    }

    public void GravityOff()
    {
        player.gravityOn = false;
        StartCoroutine(GravityCountDown());

    }
    IEnumerator GravityCountDown()
    {
        yield return new WaitForSeconds(0.1f);
        player.gravityOn = true;
    }
}
