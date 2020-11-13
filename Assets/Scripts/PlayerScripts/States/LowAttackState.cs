using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LowAttackState : PlayerState
{
    public override string GiveName()
    {
        return "LowAttackState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunLowAttackState();
        if (!AttackCheck(attackInput))
        {
            self.SetState(new IdleState());
        }
    }
}

