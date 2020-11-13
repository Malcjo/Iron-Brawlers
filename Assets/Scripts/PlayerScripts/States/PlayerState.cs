using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerState
{
    public abstract string GiveName();
    public abstract void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput);

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

