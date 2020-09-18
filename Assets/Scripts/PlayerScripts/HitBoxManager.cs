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
        }
        else if (armIndex == 1)
        {
            tempHitBox.armIndex = 1;
        }
        tempHitBox.FollowArm();
        StartCoroutine(SpawnHitBox(0.1f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void LegSweep()
    {
        tempHitBox.FollowLeg();
        StartCoroutine(SpawnHitBox(0.15f));
        StopCoroutine(SpawnHitBox(0));
    }
    IEnumerator SpawnHitBox(float spawnTime)
    {
        jab.SetActive(true);
        yield return new WaitForSeconds(spawnTime);
        jab.SetActive(false);
    }

   
}
