using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject tipHitBox,midHitBox, blockBox;
    public TempHitBox tempHitBox;
    [Range(0,1)]
    private int armIndex;
    public AnimationManager animationScript;
    public Player player;


    void Start()
    {
        animationScript = GetComponentInChildren<AnimationManager>();
        tempHitBox.HideHitBoxes();
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
        tempHitBox.FollowHand();
        StartCoroutine(SpawnHitBox(0.1f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void LegSweep()
    {
        tempHitBox.FollowRightFoot();
        StartCoroutine(SpawnHitBox(0.15f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void ArielAttack()
    {
        tempHitBox.FollowLeftFoot();
        StartCoroutine(SpawnHitBox(0.3f));
        StopCoroutine(SpawnHitBox(0));
    }
    public IEnumerator SpawnHitBox(float spawnTime)
    {
        tempHitBox.ShowHitBoxes();
        yield return new WaitForSeconds(spawnTime);
        tempHitBox.HideHitBoxes();
    }
}
