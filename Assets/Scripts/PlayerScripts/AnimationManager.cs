using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<string> animlist = new List<string>();
    public Animator anim;

    public int comboStep;
    public float comboTimer;


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
        anim.Play("JumpPrep");
        anim.SetBool("Jumping", true);
    }

    public void JumpLanding()
    {
        anim.SetBool("Jumping", false);
    }

    public void Running()
    {
        anim.SetBool("Running", true);
    }

    public void Idle()
    {
        anim.SetBool("Running", false);
    }


}
