﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : MonoBehaviour
{
    public Attackdirection _attackDir;
    public enum Attackdirection { Forward, Neutral, High, Low, Aerial };

    public AttackType _attackType;
    public enum AttackType { Jab, LegSweep, Aerial };

    private bool followArm = false;

    [SerializeField] private PlayerInput playerInput;
    private PlayerControls playerControls;

    public int armIndex;
    public GameObject rightArm, leftArm, rightFoot, leftFoot;

    private Vector3 hitDirection;

    private bool damamgeChest;
    private bool damageLegs;
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
                FollowArm();
                break;

            case AttackType.LegSweep:
                FollowRightLeg();
                break;
            case AttackType.Aerial:
                FollowLeftLeg();
                break;
            default:
                break;
        }
    }
    public void FollowArm()
    {
        if (armIndex == 0)
        {
            this.gameObject.transform.position = leftArm.transform.position;
            this.gameObject.transform.rotation = leftArm.transform.rotation;
        }
        else if (armIndex == 1)
        {
            this.gameObject.transform.position = rightArm.transform.position;
            this.gameObject.transform.rotation = rightArm.transform.rotation;
        }
    }
    public void FollowRightLeg()
    {
        this.gameObject.transform.position = rightFoot.transform.position;
        this.gameObject.transform.rotation = rightFoot.transform.rotation;
    }
    public void FollowLeftLeg()
    {
        this.gameObject.transform.position = leftFoot.transform.position;
        this.gameObject.transform.rotation = leftFoot.transform.rotation;
    }

    public Vector3 HitDirection()
    {
        switch (_attackDir)
        {
            case Attackdirection.Forward:
                return new Vector3(playerInput.FacingDirection, 0.3f, 0);
            case Attackdirection.Neutral:
                return Vector3.zero;
            case Attackdirection.High:
                return Vector3.zero;
            case Attackdirection.Low:
                return new Vector3(playerInput.FacingDirection * 0.1f, 1f,0);
            case Attackdirection.Aerial:
                return new Vector3(playerInput.FacingDirection, 0.3f, 0);
            default:
                hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
        }
    }
    
    public float HitStrength()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                return 15;
            case AttackType.LegSweep:
                return 10;
            case AttackType.Aerial:
                return 12;
            default:
                return 15;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Block")
        {
            this.gameObject.SetActive(false);
        }
        var playerChecker = other.GetComponentInParent<PlayerControls>();
        if(playerChecker.playerNumber == playerControls.playerNumber)
        {
            return;
        }
        var _tempPlayer = other.GetComponentInParent<Player>();
        if (other.gameObject.tag == "LegArmour")
        {
            _tempPlayer.RemoveLegArmour();
        }
        if(other.gameObject.tag == "ChestArmour")
        {
            _tempPlayer.RemoveChestArmour();
        }
    }
}
