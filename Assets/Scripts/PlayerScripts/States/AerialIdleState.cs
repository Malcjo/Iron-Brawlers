using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AerialIdleState : PlayerState
{

    public override string GiveName()
    {
        return "AerialIdleState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.AerialActions();

        if (MovementCheck(input.horizontalInput))
        {
            self.SetState(new AerialMovingState());
        }
        if (JumpingCheck(input.jumpInput))
        {
            self.SetState(new JumpingState());
        }
        if (self.GetVerticalState() == Player.VState.grounded)
        {
            self.SetState(new IdleState());
        }
        if (AttackCheck(input.attackInput))
        {
            self.SetState(new AerialAttackState());
        }
        if (ArmourBreakCheck(input.armourBreakInput) && CrouchingCheck(input.crouchInput))
        {
            self.SetState(new ArmourBreakState());
        }
    }
}

