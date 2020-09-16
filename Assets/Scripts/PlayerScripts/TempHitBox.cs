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
    private Player playerScript;

    public int armIndex;
    public GameObject rightArm;
    public GameObject leftArm;

    private Vector3 hitDirection;


    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerScript = GetComponentInParent<Player>();
    }
    private void Update()
    {
        if(armIndex == 0)
        {
            this.gameObject.transform.position = leftArm.transform.position;
            this.gameObject.transform.rotation = leftArm.transform.rotation;
        }
        else if(armIndex == 1)
        {
            this.gameObject.transform.position = rightArm.transform.position;
            this.gameObject.transform.rotation = rightArm.transform.rotation;
        }
    }
    public Vector3 HitDirection()
    {
        if (playerScript.grounded == true)
        {
            switch (_attackDir)
            {
                case Attackdirection.Forward:
                    return new Vector3(playerInput.FacingDirection, 0.3f, 0);
                case Attackdirection.Neutral:
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
                case Attackdirection.High:
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
                case Attackdirection.Low:
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
                default:
                    hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
            }
        }
        else
        {
            switch (_attackDir)
            {
                case Attackdirection.Forward:
                    return new Vector3(playerInput.FacingDirection, 0.3f, 0);
                case Attackdirection.Neutral:
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
                case Attackdirection.High:
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
                case Attackdirection.Low:
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
                default:
                    hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                    return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
            }
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
