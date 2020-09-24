using System.Collections;
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
    public GameObject rightArm, leftArm, rightFoot, leftFoot;

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
            case Attackdirection.Low:
                return new Vector3(playerInput.FacingDirection * 0.1f, 1f,0);
            case Attackdirection.Aerial:
                return new Vector3(playerInput.FacingDirection, 0.3f, 0);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            this.gameObject.SetActive(false);
        }
        var playerChecker = other.GetComponentInParent<PlayerControls>();
        if (playerChecker.playerNumber == playerControls.playerNumber)
        {
            return;
        }
        var _tempPlayer = other.GetComponentInParent<Player>();
        if (other.gameObject.CompareTag("LegArmour"))
        {
            _tempPlayer.RemoveLegArmour();
        }
        if (other.gameObject.CompareTag("ChestArmour"))
        {
            _tempPlayer.RemoveChestArmour();
        }
    }
}
