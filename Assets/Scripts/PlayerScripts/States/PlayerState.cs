using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerState
{
    public abstract string GiveName();
    public abstract void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput);

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
}

