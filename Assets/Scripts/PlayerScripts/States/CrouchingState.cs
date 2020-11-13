using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrouchingState : PlayerState
{
    public override string GiveName()
    {
        return "Crouching";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunCrouchingState();
        if (!CrouchingCheck(crouchInput))
        {
            self.SetState(new IdleState());
        }
        if (AttackCheck(attackInput))
        {
            self.SetState(new LowAttackState());
        }
        if (ArmourBreakCheck(armourBreakInput))
        {
            self.SetState(new ArmourBreakState());
        }
    }
}

