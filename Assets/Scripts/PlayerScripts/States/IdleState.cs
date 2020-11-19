using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleState : PlayerState
{
    public override string GiveName()
    {
        return "Idle";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
        actions.Idle();
        if (MovementCheck(input.horizontalInput))
        {
            self.SetState(new MovingState());
        }
        if (CrouchingCheck(input.crouchInput))
        {
            self.SetState(new CrouchingState());
        }
        if (JumpingCheck(input.jumpInput))
        {
            self.SetState(new JumpingState());
        }
        if (AttackCheck(input.attackInput))
        {
            self.SetState(new JabState());
        }
        if (BlockCheck(input.blockInput))
        {
            self.SetState(new BlockState());
        }

    }
}

