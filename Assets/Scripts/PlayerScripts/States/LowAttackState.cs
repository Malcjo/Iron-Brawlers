using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LowAttackState : PlayerState
{
    public override string GiveName()
    {
        return "LowAttackState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.RunLowAttackState();
        if (!AttackCheck(input.attackInput))
        {
            self.SetState(new IdleState());
        }
    }
}

