using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;
    [SerializeField] ActionManager attackManager;

    public int comboStep;
    public float comboTimer;

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
            anim.Play(animlist[comboStep]);
            comboStep++;
            comboTimer = 1;
            hitboxScript._attackDir = Attackdirection.Forward;
            hitboxScript._attackType = AttackType.Jab;
        }
    }

    public void JumpLanding(bool val)
    {
        anim.SetBool("Jumping", val);
        
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
        hitboxScript._attackDir = Attackdirection.Low;
        hitboxScript._attackType = AttackType.LegSweep;
    }

    public void AerialAttack()
    {
        anim.Play("Aerial Attack");
        hitboxScript._attackType = AttackType.Aerial;
        hitboxScript._attackDir = Attackdirection.Aerial;
    }

    public void ArmourBreak()
    {
        anim.Play("Armour Break");
        hitboxScript._attackDir = Attackdirection.Down;
        hitboxScript._attackType = AttackType.ArmourBreak;
        armourCheck.SetAllArmourOff();
        hitBoxManager.ArmourBreak();
    }
    public void Block()
    {
        hitBoxManager.Block();
    }
    public void StopBlock()
    {
        hitBoxManager.StopBlock();
    }
    public void HitStun()
    {
        anim.Play("HitStun");
    }

    public void Heavy()
    {
        anim.Play("Heavy");
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.HeavyJab;
    }
}
