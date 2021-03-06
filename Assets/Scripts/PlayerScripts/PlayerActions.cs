using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameObject[] playerGeometry, armourGeometry;
    [SerializeField] private Material normalSkinMaterial, normalSkinBlocking, armourMaterial, armourBlocking;
    public List<string> animlist = new List<string>();
    public Animator anim;
    [SerializeField] Player self;
    [SerializeField] Hitbox hitboxScript;
    [SerializeField] HitBoxManager hitboxManager;
    [SerializeField] ArmourCheck armourCheck;
    [SerializeField] ParticleSystem[] ParticleSmearLines;


    public int comboStep;
    public float comboTimer;
    private void Awake()
    {
        SetParticleTrail(false);
    }
    private void SetParticleTrail(bool control)
    {
        for (int i = 0; i < ParticleSmearLines.Length; i++)
        {
            if(control == true)
            {
                ParticleSmearLines[i].Play();
            }
            else if(control == false)
            {
                ParticleSmearLines[i].Stop();
            }
        }
    }
    private void SetParticleEmmisionToZero(bool control)
    {
        if (control == true)
        {
            for (int i = 0; i < ParticleSmearLines.Length; i++)
            {
                var emmision = ParticleSmearLines[i].main;
                emmision.startLifetime = 0.1f;

            }
        }
        else
        {
            for (int i = 0; i < ParticleSmearLines.Length; i++)
            {
                var emmision = ParticleSmearLines[i].main;
                emmision.startLifetime = 0;
            }
        }
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
        if (self.DebugModeOn == true)
        {
            Debug.Log("Jab");
        }
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
            bool canMove = true;
            //anim.Play(animlist[comboStep]);
            anim.Play("JAB");
            anim.speed = 1;
            FindObjectOfType<AudioManager>().Play(AudioManager.JABMISS);
            comboStep++;
            comboTimer = 1;
            yield return null;
            hitboxManager.SwapHands(0);
            hitboxScript._attackDir = Attackdirection.Forward;
            hitboxScript._attackType = AttackType.Jab;
            hitboxManager.JabAttack(0.5f);
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f)
                {

                    yield return null;
                }
                if (canMove == true)
                {
                    Debug.Log("Move Character");
                    self.MoveCharacterWithAttacks(200);
                }
                canMove = false;

                yield return null;
            }
            self.SetState(new IdleState());
        }
    }

    public void Heavy() 
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Heavy");
        }
        StartCoroutine(_Heavy()); 
    }
    private IEnumerator _Heavy()
    {
        bool canMove = true;
        anim.Play("HEAVY");
        FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYMISS);
        anim.speed = 1;
        yield return null;
        hitboxManager.SwapHands(1);
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.HeavyJab;
        hitboxManager.JabAttack(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
            {
                SetParticleTrail(false);
                yield return null;
                while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.65f)
                {
                    SetParticleTrail(true);
                    yield return null;
                    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f)
                    {
                        yield return null;
                    }
                    if (canMove == true)
                    {
                        Debug.Log("Move Character");
                        self.MoveCharacterWithAttacks(600);
                        canMove = false;
                    }
                }
            }
            SetParticleTrail(false);
            yield return null;
        }
        self.SetState(new IdleState());
    }
   
    public void AerialAttack()
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Aerial Attack");
        }
        StartCoroutine(_AerialAttack());
    }

    private IEnumerator _AerialAttack()
    {
        self.MoveCharacterWithAttacks(200);
        anim.Play("AERIAL");
        FindObjectOfType<AudioManager>().Play(AudioManager.AERIALMISS);
        anim.speed = 1;
        self.CanTurn = false;
        //self.UseGravity = false;
        self.StopMovingCharacterOnYAxis();
        yield return null;
        hitboxScript._attackType = AttackType.Aerial;
        hitboxScript._attackDir = Attackdirection.Aerial;
        hitboxManager.AeiralAttack();

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f)
            {
                yield return null;
            }
            //self.UseGravity = true;
            yield return null;
        }
        self.SetState(new JumpingState());
    }

    public void ForwardAerialAttack()
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Forward Aerial Attack");
        }
        self.SetState(new JumpingState());
    }
    public void BackAerialAttack()
    {
        if (self.DebugModeOn == true)
        {
            Debug.Log("Back Aerial Attack");
        }
        self.SetState(new JumpingState());
    }
    public void JumpLanding()
    {
        StartCoroutine(_Jumplanding());
    }
    private IEnumerator _Jumplanding()
    {
        anim.Play("LANDING");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.80f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }
    public void Running()
    {
        anim.Play("RUN");
        anim.speed = self.GetAbsolutInputValueForMovingAnimationSpeed();
    }
  

    public void Idle()
    {
        anim.Play("IDLE");
        anim.speed = 1;
    }

    public void Crouching()
    {
        anim.Play("CROUCH_IDLE");
        anim.speed = 1;
    }
    public void Falling()
    {
        anim.Play("FALLING");
        anim.speed = 1;
    }
    public void Jumping()
    {
        anim.Play("JUMP");
        anim.speed = 1;
    }
    public void DoubleJump(bool val)
    {
        anim.Play("DOUBLE_JUMP");
        anim.speed = 1;
    }


    public void Landing()
    {
        StartCoroutine(LandingAnim());
    }
    IEnumerator LandingAnim()
    {
        self.landing = true;
        anim.Play("LANDING");
        anim.speed = 2;
        self.SetState(new BusyState());
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.landing = false;
        self.SetState(new IdleState());
    }
    public void LegSweep()
    {
        StartCoroutine(_LegSweep());
    }
    private IEnumerator _LegSweep()
    {
        anim.Play("SWEEP");
        anim.speed = 1;
        self.CanTurn = false;
        yield return null;
        hitboxManager.SwapHands(1);
        hitboxScript._attackDir = Attackdirection.Low;
        hitboxScript._attackType = AttackType.LegSweep;
        hitboxManager.LegSweep(0.5f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }

    public void PauseCurrentAnimation()
    {
        anim.speed = 0;
    }
    public void ResumeCurrentAnimation()
    {
        anim.speed = 1;
    }

    public void ArmourBreak()
    {
        StartCoroutine(_ArmourBreak());
    }
    private IEnumerator _ArmourBreak()
    {
        if (armourCheck.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none && armourCheck.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none)
        {
            RevertBackToIdleState();
            yield return null;
        }
        else
        {
            anim.Play("ARMOUR_BREAK");
            anim.speed = 1;
            FindObjectOfType<AudioManager>().Play(AudioManager.ARMOURBREAK);
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
            RevertBackToIdleState();
        }
    }
    private void RevertBackToIdleState()
    {
        if (self.VerticalState == Player.VState.grounded)
        {
            Debug.Log("End hit stun");
            self.SetState(new IdleState());

        }
        else
        {
            self.SetState(new JumpingState());
        }
    }
    public void Block()
    {
        StartCoroutine(EnterBlock());
    }
    IEnumerator EnterBlock()
    {
        for (int i = 0; i < playerGeometry.Length; i++)
        {
            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalSkinBlocking;
        }
        for (int i = 0; i < armourGeometry.Length; i++)
        {
            var tempMaterial = armourGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourBlocking;
        }
        hitboxManager.Block();
        anim.Play("BLOCK");
        yield return null;
        while(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        StartCoroutine(BlockIdle());
    }
    IEnumerator BlockIdle()
    {
        anim.Play("BLOCK_IDLE");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }
    public void StopBlock()
    {
        StartCoroutine(ExitBlock());
    }
    public void ResetMaterial()
    {
        for (int i = 0; i < playerGeometry.Length; i++)
        {
            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalSkinMaterial;
        }
        for (int i = 0; i < armourGeometry.Length; i++)
        {
            var tempMaterial = armourGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
    }
    IEnumerator ExitBlock()
    {
        for (int i = 0; i < playerGeometry.Length; i++)
        {
            var tempMaterial = playerGeometry[i].GetComponent<SkinnedMeshRenderer>();
            tempMaterial.material = normalSkinMaterial;
        }
        for (int i = 0; i < armourGeometry.Length; i++)
        {
            var tempMaterial = armourGeometry[i].GetComponent<MeshRenderer>();
            tempMaterial.material = armourMaterial;
        }
        hitboxManager.StopBlock();
        anim.Play("BLOCK_EXIT");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.SetState(new IdleState());
    }
    public void HitKnockBack()
    {
        StartCoroutine(KnockBack());
    }
    IEnumerator KnockBack()
    {
        self.HitStun = true;
        self.SetState(new BusyState());
        yield return null;
        anim.Play("HITSTUN_NORMAL_HIT");
        anim.speed = 2;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        self.HitStun = false;
        self.SetState(new IdleState());
    }
    public void HitStunKnockDown()
    {
        StartCoroutine(KnockDownStun());
    }
    private IEnumerator KnockDownStun()
    {
        self.HitStun = true;
        self.CanTurn = false;
        self.SetState(new BusyState());
        //anim.Play("HITSTUN_NORMAL_HIT");
        anim.Play("KNOCKDOWN_NORMAL");
        anim.speed = 2.5f;
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        StartCoroutine(GetBackUp());

    }
    private IEnumerator GetBackUp()
    {
        anim.Play("GETTING_UP_NORMAL");
        anim.speed = 2;
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        self.HitStun = false;
        RevertBackToIdleState();
    }
}
