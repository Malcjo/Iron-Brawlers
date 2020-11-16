using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumpingState : PlayerState
{
    public override string GiveName()
    {
        return "Jumping";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.RunJumpingState();
        self.SetState(new AerialIdleState());
        if (MovementCheck(input.horizontalInput))
        {
            self.SetState(new AerialMovingState());
        }
    }
}

