using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixamoAnimations : MonoBehaviour
{
    public Animator anim;


    public void Jab()
    {
        //jab animation has event that calls JabAttack() on PlayerAttack script setting hitboxes.
        anim.SetTrigger("Jab");
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
