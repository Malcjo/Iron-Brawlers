using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AirborneIdleState : PlayerState
{
    public override string GiveName()
    {
        return "AirborneIdle";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput)
    {
        if (MovementCheck(horizontalInput))
        {
            self.SetState(new MovingState());
        }
        if (JumpingCheck(jumpInput))
        {
            self.SetState(new JumpingState());
        }
        if (self.currentVerticalState == Player.VState.grounded)
        {
            self.SetState(new IdleState());
        }
    }
}

