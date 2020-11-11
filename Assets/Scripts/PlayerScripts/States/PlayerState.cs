using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerState
{
    public abstract string GiveName();
    public abstract void RunState(Player self, float horizontalInput, bool attackInput, bool crouchInput, bool jumpInput);

    protected bool MovementCheck(float horizontalInput)
    {
        return horizontalInput != 0;
    }
    protected bool CrouchingCheck(bool crouchInput)
    {
        return crouchInput;
    }
}

