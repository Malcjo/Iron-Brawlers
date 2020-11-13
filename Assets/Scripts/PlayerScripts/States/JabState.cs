using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JabState : PlayerState
{
    public override string GiveName()
    {
        return "JabState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunJabState();
        if (!AttackCheck(attackInput))
        {
            self.SetState(new IdleState());
        }
    }
}

