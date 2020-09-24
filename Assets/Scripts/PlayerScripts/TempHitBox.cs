﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : MonoBehaviour
{
    public enum Attackdirection { Forward, Low, Aerial };
    public Attackdirection _attackDir;

    public enum AttackType { Jab, LegSweep, Aerial };
    public AttackType _attackType;

    PlayerInput playerInput;
    PlayerControls playerControls;

    public int armIndex;
    public GameObject tipHitBox, midHitBox;
    public GameObject rightHand, leftHand,rightElbow, leftElbow, rightFoot, leftFoot, rightKnee, leftKnee;

    Vector3 hitDirection;

    private void Awake()
    {
        playerControls = GetComponentInParent<PlayerControls>();
        playerInput = GetComponentInParent<PlayerInput>();
    }
    private void Update()
    {
        AttackTypeCall();
    }
    void AttackTypeCall()
    {
        switch(_attackType)
        {
            case AttackType.Jab:
                FollowHand();
                break;
            case AttackType.LegSweep:
                FollowRightFoot();
                break;
            case AttackType.Aerial:
                FollowLeftFoot();
                break;
            default:
                break;
        }
    }
    public void FollowHand()
    {
        if (armIndex == 0)
        {
            tipHitBox.gameObject.transform.position = leftHand.transform.position;
            tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
            midHitBox.gameObject.transform.position = leftElbow.transform.position;
            midHitBox.gameObject.transform.rotation = leftElbow.transform.rotation;
        }
        else if (armIndex == 1)
        {
            tipHitBox.gameObject.transform.position = rightHand.transform.position;
            tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
            midHitBox.gameObject.transform.position = rightElbow.transform.position;
            midHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
        }
    }
    public void FollowRightFoot()
    {
        tipHitBox.gameObject.transform.position = rightFoot.transform.position;
        tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
        midHitBox.gameObject.transform.position = rightKnee.transform.position;
        midHitBox.gameObject.transform.rotation = rightKnee.transform.rotation;
    }
    public void FollowLeftFoot()
    {
        tipHitBox.gameObject.transform.position = leftFoot.transform.position;
        tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
        midHitBox.gameObject.transform.position = leftKnee.transform.position;
        midHitBox.gameObject.transform.rotation = leftKnee.transform.rotation;
    }

    public Vector3 HitDirection()
    {
        switch (_attackDir)
        {
            case Attackdirection.Forward:
                return new Vector3(playerInput.FacingDirection, 0.3f, 0);
            case Attackdirection.Low:
                return new Vector3(playerInput.FacingDirection * 0.1f, 1f,0);
            case Attackdirection.Aerial:
                return new Vector3(playerInput.FacingDirection, 0.7f, 0);
            default:
                hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                return new Vector3(playerInput.FacingDirection, 0.3f, 0);
        }
    }
    
    public float HitStrength()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                return 15;
            case AttackType.LegSweep:
                return 12;
            case AttackType.Aerial:
                return 10;
            default:
                return 15;
        }
    }
    public void ShowHitBoxes()
    {
        tipHitBox.gameObject.SetActive(true);
        midHitBox.gameObject.SetActive(true);
    }
    public void HideHitBoxes()
    {
        tipHitBox.gameObject.SetActive(false);
        midHitBox.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            return;
        }
        if (other.gameObject.CompareTag("Block"))
        {
            HideHitBoxes();
        }
        var _tempArmour = other.GetComponentInParent<ArmourCheck>();
        if (other.gameObject.CompareTag("LegArmour"))
        {
            _tempArmour.RemoveLegArmour();
        }
        if (other.gameObject.CompareTag("ChestArmour"))
        {
            _tempArmour.RemoveChestArmour();
        }
    }
}
