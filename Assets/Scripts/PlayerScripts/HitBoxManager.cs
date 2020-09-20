using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject hitBox, blockBox;
    public TempHitBox tempHitBox;
    [Range(0,1)]
    private int armIndex;
    public AnimationManager animationScript;
    public Player player;


    void Start()
    {
        animationScript = GetComponentInChildren<AnimationManager>();
        hitBox.SetActive(false);
        blockBox.SetActive(false);
    }
    public void Block()
    {
        blockBox.SetActive(true);
        player.blocking = true;
    }
    public void StopBlock()
    {
        blockBox.SetActive(false);
        player.blocking = false;
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
    public IEnumerator SpawnHitBox(float spawnTime)
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(spawnTime);
        hitBox.SetActive(false);
    }
}
