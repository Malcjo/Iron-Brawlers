using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : MonoBehaviour
{
    public Attacktype attackIndex;
    public PlayerInput playerInput;
    public enum Attacktype {Forward, Neutral, High, Low};
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }
    public Vector3 HitDirection()
    {
        switch (attackIndex)
        {
            case Attacktype.Forward:
                return new Vector3(1, 0, 0) * playerInput.FacingDirection;
            case Attacktype.Neutral:
                return new Vector3(0.5f, 0, 0) * playerInput.FacingDirection;
            case Attacktype.High:
                return new Vector3(0, -1, 0) * playerInput.FacingDirection;
            case Attacktype.Low:
                return new Vector3(0, 1, 0) * playerInput.FacingDirection;
            default:
                return new Vector3(1, 0, 0) * playerInput.FacingDirection;
        }
    }
}
