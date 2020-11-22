using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;
    [SerializeField] Player self;
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
            self.SetState(new IdleState());
        }
        else if (comboStep < (animlist.Count))
        {
            anim.Play("JAB");
            comboStep++;
            comboTimer = 1;
            yield return null;

            hitboxScript._attackDir = Attackdirection.Forward;
            hitboxScript._attackType = AttackType.Jab;
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            self.SetState(new IdleState());
        }

    }

    public void JumpLanding()
    {
        anim.Play("LANDING");
    }

    
    public void Running()
    {
        anim.Play("RUN");
    }

    public void Idle()
    {
        anim.Play("IDLE");
    }

    public void Crouching()
    {
        anim.Play("CROUCH_IDLE");
    }
    public void Falling()
    {
        anim.Play("FALLING");
    }
    public void Jumping()
    {
        anim.Play("JUMP");
    }
    public void DoubleJump(bool val)
    {
        anim.SetBool("DOUBLE_JUMP", val);
    }

    public void LegSweep()
    {
        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        anim.Play("SWEEP");
        self.CanTurn = false;
        yield return null;
        hitboxScript._attackDir = Attackdirection.Low;
        hitboxScript._attackType = AttackType.LegSweep;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }
    public void AerialAttack()
    {
        StartCoroutine(_AerialAttack());
    }
    private IEnumerator _AerialAttack()
    {
        anim.Play("AERIAL");
        self.CanTurn = false;
        yield return null;
        hitboxScript._attackType = AttackType.Aerial;
        hitboxScript._attackDir = Attackdirection.Aerial;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }

    public void ArmourBreak()
    {
        StartCoroutine(_ArmourBreak());
    }
    private IEnumerator _ArmourBreak()
    {
        anim.Play("ARMOUR_BREAK");
        self.CanTurn = false;
        yield return null;
        hitboxScript._attackDir = Attackdirection.Down;
        hitboxScript._attackType = AttackType.ArmourBreak;
        armourCheck.SetAllArmourOff();
        hitboxManager.ArmourBreak();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        if(self.GetVerticalState() == Player.VState.grounded)
        {
            self.SetState(new IdleState());
        }
        else
        {
            self.SetState(new AerialIdleState());
        }

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
        anim.Play("HITSTUN_NORMAL_HIT");
    }

    public void Heavy()
    {
        StartCoroutine(_Heavy());
    }
    private IEnumerator _Heavy()
    {
        anim.Play("HEAVY");
        yield return null;
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.HeavyJab;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }
}
