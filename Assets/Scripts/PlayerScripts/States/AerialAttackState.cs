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
        actions.AerialAttack();
        self.CanTurn = false;
        self.WasAttacking = true;
        self.SetState(new BusyState());
    }
}

