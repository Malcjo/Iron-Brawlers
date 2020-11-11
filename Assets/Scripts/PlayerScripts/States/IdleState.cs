using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleState : PlayerState
{
    public override string GiveName()
    {
        return "Idle";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput)
    {
        self.RunIdleState();
        if (MovementCheck(horizontalInput))
        {
            self.SetState(new MovingState());
        }
        if (CrouchingCheck(crouchInput))
        {
            self.SetState(new CrouchingState());
        }
        if (JumpingCheck(jumpInput))
        {
            self.SetState(new JumpingState());
        }

    }
}

