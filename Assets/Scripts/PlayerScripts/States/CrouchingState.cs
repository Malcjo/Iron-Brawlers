using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrouchingState : PlayerState
{
    public override string GiveName()
    {
        return "Crouching";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
        actions.Crouching();
        if (!CrouchingCheck(input.crouchInput))
        {
            self.SetState(new IdleState());
        }
        if (AttackCheck(input.attackInput))
        {
            self.SetState(new LowAttackState());
        }
        if (ArmourBreakCheck(input.armourBreakInput))
        {
            self.SetState(new ArmourBreakState());
        }
    }
}

