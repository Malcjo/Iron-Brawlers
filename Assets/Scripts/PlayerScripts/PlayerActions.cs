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
    private Player.VState verticalState;

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
        StartCoroutine(Jab());
    }
    
    private IEnumerator Jab()
    {
        if (comboStep >= (animlist.Count))
        {
            yield return null;
            player.SetState(new IdleState());
        }
        else if (comboStep < (animlist.Count))
        {
            anim.Play("Jab");
            comboStep++;
            comboTimer = 1;
            yield return null;

            hitboxScript._attackDir = Attackdirection.Forward;
            hitboxScript._attackType = AttackType.Jab;
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            player.SetState(new IdleState());
        }

    }

    public void JumpLanding()
    {
        anim.Play("Jumping");
    }

    
    public void Running()
    {
        anim.Play("Run");
    }

    public void Idle()
    {
        anim.Play("Idle");
    }

    public void Crouching()
    {
        anim.Play("Crouch");
    }
    public void Falling()
    {
        anim.Play("Falling");
    }
    public void Jumping()
    {
        anim.Play("Jumping");
    }
    public void DoubleJump(bool val)
    {
        anim.SetBool("DoubleJump", val);
    }

    public void LegSweep()
    {
        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        anim.Play("Leg Sweep");
        yield return null;
        hitboxScript._attackDir = Attackdirection.Low;
        hitboxScript._attackType = AttackType.LegSweep;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        player.SetState(new IdleState());
    }
    public void AerialAttack()
    {
        StartCoroutine(_AerialAttack());
    }
    private IEnumerator _AerialAttack()
    {
        anim.Play("Aerial Attack");
        yield return null;
        hitboxScript._attackType = AttackType.Aerial;
        hitboxScript._attackDir = Attackdirection.Aerial;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        player.SetState(new IdleState());
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
