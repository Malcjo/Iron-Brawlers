using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmourBreakState : PlayerState
{
    public override string GiveName()
    {
        return "ArmourBreakState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunArmourBreakState();
        if (!AttackCheck(attackInput))
        {
            self.SetState(new IdleState());
        }
    }
}

