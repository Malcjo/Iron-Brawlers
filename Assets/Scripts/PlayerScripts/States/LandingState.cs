using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandingState : PlayerState
{
    public override string GiveName()
    {
        return "LandingState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.InAir = false;
        actions.JumpLanding();
        if(self.WasAttacking == true)
        {
            self.WasAttacking = false;
            self.CanMove = false;
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
        }
        else
        {
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
                self.CanMove = false;
                body.velocity = new Vector3(0, body.velocity.y, 0);
                self.CanTurn = false;
                actions.JabCombo();
                self.SetState(new BusyState());
            }
            if (BlockCheck(input.blockInput))
            {
                self.SetState(new BlockState());
            }
        }
    }
}

