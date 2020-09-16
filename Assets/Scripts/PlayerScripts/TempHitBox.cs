using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : MonoBehaviour
{
    public Attackdirection _attackDir;
    public enum Attackdirection { Forward, Neutral, High, Low };

    public AttackType _attackType;
    public enum AttackType { Jab, LegSweep };


    public PlayerInput playerInput;

    public int armIndex;
    public GameObject rightArm, leftArm;

    private Vector3 hitDirection;


    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }
    private void Update()
    {
        HitBoxFollowArm();
    }

    void HitBoxFollowArm()
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
                return Vector3.zero;
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
                return 12;
            default:
                return 15;
        }
    }

}
