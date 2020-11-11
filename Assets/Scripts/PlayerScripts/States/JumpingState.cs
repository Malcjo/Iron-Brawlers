using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumpingState : PlayerState
{
    public override string GiveName()
    {
        return "Jumping";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput)
    {
        self.RunJumpingState();
        if (!JumpingCheck(jumpInput))
        {
            self.SetState(new AirborneIdleState());
        }

    }
}

