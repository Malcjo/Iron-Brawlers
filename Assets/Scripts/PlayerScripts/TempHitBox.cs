using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : MonoBehaviour
{
    public Attacktype attackIndex;
    public PlayerInput playerInput;
    public GameObject rightArm;
    public GameObject leftArm;

    private Vector3 hitDirection;
    public enum Attacktype {Forward, Neutral, High, Low};
    public int armIndex;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
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
        switch (attackIndex)
        {
            case Attacktype.Forward:
                hitDirection.x = 1f; hitDirection.y = 0.2f; hitDirection.z = 0; ;
                return new Vector3 (hitDirection.x * playerInput.FacingDirection, hitDirection.y, hitDirection.z);
            case Attacktype.Neutral:
                return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, hitDirection.z);
            case Attacktype.High:
                return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, hitDirection.z);
            case Attacktype.Low:
                return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, hitDirection.z);
            default:
                hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, hitDirection.z);
        }
    }
}
