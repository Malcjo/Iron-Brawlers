using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovingState : PlayerState
{
    public override string GiveName()
    {
        return "Moving";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunMoveState();
        if (!MovementCheck(horizontalInput))
        {
            self.SetState(new IdleState());
        }
        if (CrouchingCheck(crouchInput))
        {
            self.SetState(new CrouchingState());
        }
        if (JumpingCheck(jumpInput))
        {
            self.SetState(new JumpingState());
            Debug.Log("Jump While moving");
        }
    }
}

