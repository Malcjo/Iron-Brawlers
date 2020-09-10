using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject jab;
    AnimationManager animationScript;


    void Start()
    {
        animationScript = GetComponentInChildren<AnimationManager>();
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
