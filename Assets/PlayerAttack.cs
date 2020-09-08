using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject jab;
    MixamoAnimations animationScript;


    void Start()
    {
        animationScript = GetComponentInChildren<MixamoAnimations>();
        jab.SetActive(false);
    }


    public void JabAttack()
    {
        StartCoroutine(Jab());
        StopCoroutine(Jab());
    }
    IEnumerator Jab()
    {
        jab.SetActive(true);
        yield return new WaitForSeconds(0.0001f);
        jab.SetActive(false);
    }

   
}
