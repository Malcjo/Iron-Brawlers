using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AerialAttackState : PlayerState
{
    public override string GiveName()
    {
        return "AerialAttackState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunNeutralAirState();
        if (!AttackCheck(attackInput))
        {
            self.SetState(new AerialIdleState());
        }
    }
}

