using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AerialIdleState : PlayerState
{
    public override string GiveName()
    {
        return "AerialIdleState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        if (MovementCheck(horizontalInput))
        {
            self.SetState(new AerialMovingState());
        }
        if (JumpingCheck(jumpInput))
        {
            self.SetState(new JumpingState());
        }
        if (self.GetVerticalState() == Player.VState.grounded)
        {
            self.SetState(new IdleState());
        }
        if (AttackCheck(attackInput))
        {
            self.SetState(new AerialAttackState());
        }
    }
}

