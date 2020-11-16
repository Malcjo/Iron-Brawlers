using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AerialAttackState : PlayerState
{
    public override string GiveName()
    {
        return "AerialAttackState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.RunNeutralAirState();
        if (!AttackCheck(input.attackInput))
        {
            self.SetState(new AerialIdleState());
        }
    }
}

