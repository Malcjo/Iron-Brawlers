using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerState
{
    public struct InputState
    {
        public float horizontalInput;
        public bool attackInput;
        public bool jumpInput;
        public bool crouchInput;
        public bool armourBreakInput;
        public bool blockInput;
    }
    public struct Calculating
    {
        public float jumpForce;
        public float friction;
        public float characterSpeed;
        public Vector3 addForce;
    }
    public abstract string GiveName();
    public abstract void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate);

    protected bool MovementCheck(float horizontalInput)
    {
        return horizontalInput != 0;
    }
    protected bool CrouchingCheck(bool crouchInput)
    {
        return crouchInput;
    }
    protected bool JumpingCheck(bool jumpInput)
    {
        return jumpInput;
    }
    protected bool AttackCheck(bool attackInput)
    {
        return attackInput;
    }
    protected bool ArmourBreakCheck(bool armourBreakInput)
    {
        return armourBreakInput;
    }
    protected bool BlockCheck(bool blockInput)
    {
        return blockInput;
    }
}

