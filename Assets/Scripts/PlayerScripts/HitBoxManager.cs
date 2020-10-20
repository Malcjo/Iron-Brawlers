using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject tipHitBox, blockBox;
    public Hitbox hitBox;
    [Range(0,1)]
    private int armIndex;
    public AnimationManager animationScript;
    public Player player;


    void Start()
    {
        animationScript = GetComponentInChildren<AnimationManager>();
        hitBox.HideHitBoxes();
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
        hitBox._hitBoxScale = HitBoxScale.Jab;
        armIndex = _armIndex;
        if (armIndex == 0)
        {
            hitBox.armIndex = 0;
        }
        else if (armIndex == 1)
        {
            hitBox.armIndex = 1;
        }
        hitBox.FollowHand();//to snap into place before hitbox is played
        StartCoroutine(SpawnHitBox(0.1f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void LegSweep()
    {
        hitBox.FollowRightFoot();//to snap into place before hitbox is played
        hitBox._hitBoxScale = HitBoxScale.Jab;
        StartCoroutine(SpawnHitBox(0.15f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void ArielAttack()
    {
        hitBox.FollowLeftFoot();//to snap into place before hitbox is played
        hitBox._hitBoxScale = HitBoxScale.Jab;
        StartCoroutine(SpawnHitBox(0.3f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void ShineAttack()
    {
        hitBox.FollowCenter();//to snap into place before hitbox is played
        hitBox._hitBoxScale = HitBoxScale.Shine;
        StartCoroutine(FreezeFrames(0.1f, 0.1f));
        StartCoroutine(SpawnHitBox(0.25f));
        StopCoroutine(FreezeFrames(0, 0));
        StopCoroutine(SpawnHitBox(0));
    }
    public IEnumerator SpawnHitBox(float spawnTime)
    {
        hitBox.ShowHitBoxes();
        yield return new WaitForSeconds(spawnTime);
        hitBox.HideHitBoxes();
    }

    public IEnumerator FreezeFrames(float delayTime, float AnimationTime)
    {
        yield return new WaitForSeconds(delayTime);
        player.inAnimation = true;
        yield return new WaitForSeconds(AnimationTime);
        player.inAnimation = false;
    }
}
