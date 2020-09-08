using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixamoAnimations : MonoBehaviour
{
    public Animator anim;


    public void Jab()
    {
        anim.SetTrigger("Jab");
    }

    public void Running()
    {
        anim.SetBool("Running", true);
    }


}
