using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrouchingState : PlayerState
{
    public override string GiveName()
    {
        return "Crouching";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool crouchInput, bool jumpInput)
    {
        self.RunMoveState();
        if (!MovementCheck(horizontalInput))
        {
            self.SetState(new IdleState());
        }
        if (!CrouchingCheck(crouchInput))
        {
            self.SetState(new IdleState());
        }
    }
}

