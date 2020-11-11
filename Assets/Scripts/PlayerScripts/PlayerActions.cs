using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;
    [SerializeField] Player player;
    [SerializeField] Hitbox hitboxScript;
    [SerializeField] HitBoxManager hitboxManager;
    [SerializeField] ArmourCheck armourCheck;

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
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animlist[comboStep]))
            {
                //player.CurrentState = Player.State.busy;
            }
        }
    }

    public void JumpLanding()
    {
        anim.Play("Jumping");
    }
    
    public void Running()
    {
        anim.Play("Fast Run");
    }

    public void Idle()
    {
        anim.Play("Idle");
    }

    public void Crouching()
    {
        anim.Play("isCrouching");
    }

    public void Jump(bool val)
    {
        if(val == true)
        {
            anim.Play("Jumping");
        }
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
        hitboxManager.ArmourBreak();
    }
    public void Block()
    {
        hitboxManager.Block();
    }
    public void StopBlock()
    {
        hitboxManager.StopBlock();
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
