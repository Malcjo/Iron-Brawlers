using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject tipHitBox, blockBox;
    public Hitbox hitBox;
    [Range(0,1)]
    private int armIndex;
    public PlayerActions animationScript;
    public Player player;
    private PlayerInput playerInput;
    public Vector3 blockOffset;


    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        animationScript = GetComponentInChildren<PlayerActions>();
        hitBox.HideHitBoxes();
        blockBox.SetActive(false);
    }
    public void Block()
    {
        blockBox.transform.position = transform.position + new Vector3(playerInput.FacingDirection * blockOffset.x, blockOffset.y, 0);
        blockBox.SetActive(true);
        player.blocking = true;
    }
    public void StopBlock()
    {
        blockBox.SetActive(false);
        player.blocking = false;
    }
    public void SwapHands(int _armIndex)
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
    }
    public void JabAttack(float spawnTime)
    {

        StartCoroutine(SpawnHitBox(spawnTime));
        StopCoroutine(SpawnHitBox(0));
    }
    public void LegSweep()
    {
        hitBox.FollowHand();//to snap into place before hitbox is played
        hitBox._hitBoxScale = HitBoxScale.Aerial;
        StartCoroutine(SpawnHitBox(0.15f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void AeiralAttack()
    {
        hitBox.FollowRightElbow();//to snap into place before hitbox is played
        hitBox._hitBoxScale = HitBoxScale.Aerial;
        StartCoroutine(SpawnHitBox(0.3f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void ArmourBreak()
    {
        hitBox.FollowCenter();//to snap into place before hitbox is played
        hitBox._hitBoxScale = HitBoxScale.ArmourBreak;
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
