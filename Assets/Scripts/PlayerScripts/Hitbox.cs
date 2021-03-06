﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Jab, LegSweep, Aerial, ArmourBreak, HeavyJab };
public enum Attackdirection { Forward, Low, Aerial, Down };
public enum HitBoxScale { Jab, ArmourBreak, Aerial };
public enum FollowDes { Centre, RightHand, RightElbow, LeftHand, LeftElbow , RightFoot, LeftFoot}
public class Hitbox : MonoBehaviour
{
    private float gaugeDamageValue = 1.5f;
    public bool viewHitBox;
    public HitBoxScale _hitBoxScale;
    public Attackdirection _attackDir;
    public AttackType _attackType;
    private FollowDes _followDes;
    MeshRenderer meshRenderer;
    Collider hitboxCollider;

    [SerializeField] private float freezeCounter;
    [SerializeField] private float freezeStep;
    [SerializeField] private float freezeMaxValue;
    public PlayerActions animantionManager;
    public Animator anim;
    [SerializeField] Player player;
    PlayerInputHandler playerInput;
    HitBoxManager hitBoxManager;

    [SerializeField] private bool freezeCharacter;
    public List<GameObject> HitHurtBoxes = new List<GameObject>();
    public List<GameObject> HurtboxList = new List<GameObject>();

    public int armIndex;
    public GameObject tipHitBox, midHitBox;
    public GameObject rightHand, leftHand,rightElbow, leftElbow, rightFoot, leftFoot, rightKnee, leftKnee, waist;

    public ParticleSystem hitParticle;
    public ParticleSystem dustHitParticle;

    Vector3 hitDirection;

    public void AddHurtBoxToList(GameObject obj)
    {
        HurtboxList.Add(obj);
    }
    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        hitBoxManager = GetComponent<HitBoxManager>();
        playerInput = GetComponentInParent<PlayerInputHandler>();
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        HideHitBoxes();
    }
    private void FixedUpdate()
    {
        AttackTypeCall();
        HitBoxSize();
    }
    void AttackTypeCall()
    {
        switch(_attackType)
        {
            case AttackType.Jab:
                FollowHand();
                break;
            case AttackType.LegSweep:
                FollowHand();
                break;
            case AttackType.Aerial:
                FollowRightElbow();
                break;
            case AttackType.ArmourBreak:
                FollowCenter();
                break;
            case AttackType.HeavyJab:
                FollowHand();
                break;
        }
    }
    void HitBoxSize()
    {
        switch (_hitBoxScale)
        {
            case HitBoxScale.Jab:
                transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                break;
            case HitBoxScale.ArmourBreak:
                transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                break;
            case HitBoxScale.Aerial:
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
        }
    }
    void HitboxPosition()
    {
        switch(_followDes)
        {
            case FollowDes.Centre:
                break;
            case FollowDes.RightHand:
                tipHitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
                break;
            case FollowDes.RightElbow:
                tipHitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
                break;
            case FollowDes.RightFoot:
                tipHitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
                break;
            case FollowDes.LeftHand:
                tipHitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
                break;
            case FollowDes.LeftElbow:
                break;
            case FollowDes.LeftFoot:
                tipHitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
                break;
        }
    }
    public void FollowCenter()
    {
        tipHitBox.gameObject.transform.position = waist.transform.position;
        tipHitBox.gameObject.transform.rotation = waist.transform.rotation;
    }
    public void FollowHand()
    {
        if (armIndex == 0)
        {
            tipHitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
            tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
        }
        else if (armIndex == 1)
        {
            tipHitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
            tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
        }
    }
    public void FollowRightElbow()
    {
        tipHitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
    }
    public void FollowRightFoot()
    {
        tipHitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
    }
    public void FollowLeftFoot()
    {
        tipHitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
    }
    public Vector3 KnockBackStrength()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                return new Vector3(player.GetFacingDirection() * 5, 2, 0);
            case AttackType.LegSweep:
                return new Vector3(player.GetFacingDirection() * 5, 7, 0);
            case AttackType.Aerial:
                return new Vector3(player.GetFacingDirection() * 15, -2, 0);
            case AttackType.ArmourBreak:
                return new Vector3(player.GetFacingDirection() * 5, 2, 0);
            case AttackType.HeavyJab:
                return new Vector3(player.GetFacingDirection() * 20, 2, 0);
        }
        return new Vector3(player.GetFacingDirection() * 5, 2, 0);
    }

    public void ShowHitBoxes()
    {
        //meshRenderer.enabled = true;
        hitboxCollider.enabled = true;
    }
    public void HideHitBoxes()
    {
        meshRenderer.enabled = false;
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            return;
        }


        if (other.gameObject.CompareTag("Hurtbox"))
        {
            
            var tempDefendingPlayer = other.GetComponentInParent<Player>();
            var tempAttackingPlayer = GetComponentInParent<Player>();
            var temptArmourCheck = other.GetComponentInParent<ArmourCheck>();
            HurtBox tempHurtBox = other.gameObject.GetComponent<HurtBox>();
            tempHurtBox.TurnOnHitBoxHit();
            Debug.Log("Hit Character");
            if (tempDefendingPlayer.Blocking == true)
            {
                if(_attackType != AttackType.LegSweep)
                {
                    Debug.Log("Blocked");
                    HideHitBoxes();
                    return;
                }
                else
                {
                    DamagingPlayer(tempDefendingPlayer, tempAttackingPlayer, temptArmourCheck, tempHurtBox);
                    tempDefendingPlayer.HideHitBoxes();
                    tempDefendingPlayer.ResetCharacterMaterialToStandard();
                }
            }
            else
            {
                DamagingPlayer(tempDefendingPlayer, tempAttackingPlayer, temptArmourCheck, tempHurtBox);
                tempDefendingPlayer.HideHitBoxes();
            }
        }
    }
    private void DamagingPlayer(Player DefendingPlayer, Player attackingPlayer, ArmourCheck armourCheck, HurtBox hurtBox)
    {
        if (hurtBox.BodyLocation == LocationTag.Chest)
        {
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Chest, _attackType);
            Instantiate(hitParticle, transform.position, transform.rotation);
            Instantiate(dustHitParticle, transform.position, transform.rotation);

        }
        else if (hurtBox.BodyLocation == LocationTag.Legs)
        {
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Legs, _attackType);
            Instantiate(hitParticle, transform.position, transform.rotation);
            Instantiate(dustHitParticle, transform.position, transform.rotation);
        }

        ApplyDamageToPlayer(DefendingPlayer, attackingPlayer, _attackType);
    }
    void ApplyDamageToPlayer(Player defendingPlayer, Player attackingPlayer, AttackType attackType)
    {
        defendingPlayer.FreezeCharacterBeingAttacked(KnockBackStrength());
        attackingPlayer.FreezeCharacterAttacking();
        if(attackType == AttackType.Aerial || attackType == AttackType.ArmourBreak || attackType == AttackType.HeavyJab)
        {
            defendingPlayer.KnockDown();
        }
        else if(attackType == AttackType.LegSweep)
        {
            defendingPlayer.KnockDown();

        }
        else if(attackType == AttackType.Jab)
        {
            defendingPlayer.KnockBack();
        }

        HideHitBoxes();
    }
}
