using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject jab;
    public TempHitBox tempHitBox;
    [Range(0,1)]
    private int armIndex;
    AnimationManager animationScript;


    void Start()
    {
        animationScript = GetComponentInChildren<AnimationManager>();
        jab.SetActive(false);
    }


    public void JabAttack(int _armIndex)
    {

        armIndex = _armIndex;
        if (armIndex == 0)
        {
            tempHitBox.armIndex = 0;
            jab.transform.position = tempHitBox.leftArm.transform.position;
            jab.transform.rotation = tempHitBox.leftArm.transform.rotation;
        }
        else if (armIndex == 1)
        {
            tempHitBox.armIndex = 1;
            jab.transform.position = tempHitBox.rightArm.transform.position;
            jab.transform.rotation = tempHitBox.rightArm.transform.rotation;
        }

        StartCoroutine(Jab());
        StopCoroutine(Jab());
    }
    IEnumerator Jab()
    {
        jab.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        jab.SetActive(false);
    }

   
}
