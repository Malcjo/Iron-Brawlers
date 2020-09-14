using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    PlayerControls controls;
    Player playerScript;

    [SerializeField]
    bool canDoubleJump;

    public int comboStep;
    public float comboTimer;

    private void Start()
    {
        controls = GetComponentInParent<PlayerControls>();
        playerScript = GetComponentInParent<Player>();

        canDoubleJump = false;
    }
    private void Update()
    {
        if(comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer < 0)
            {
                comboStep = 0;
            }
        }

        if (Input.GetKeyDown(controls.jumpKey) && playerScript.grounded == false)
        {
            anim.Play("DoubleJump");
        }

    }

    public void JabCombo()
    {
        if(comboStep < animlist.Count)
        {
            anim.Play(animlist[comboStep]);
            comboStep++;
            comboTimer = 1;
        }
    }
    
    public void JumpPrep()
    {
        anim.SetBool("Jumping", true);
        //canDoubleJump = true;
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
