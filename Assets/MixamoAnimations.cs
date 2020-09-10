using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixamoAnimations : MonoBehaviour
{
    public Animator anim;
    bool comboPossible;
    public int comboStep;


    public void Jab()
    {
        if(comboStep == 0)
        {
           anim.SetTrigger("Punch1");
            comboStep = 1;
            return;
        }
        if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if(comboStep == 2)
            anim.SetTrigger("Punch2");
        if (comboStep == 3)
            anim.SetTrigger("Punch3");
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }

    
    public void Jumping()
    {
        anim.SetTrigger("Jumping");
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
