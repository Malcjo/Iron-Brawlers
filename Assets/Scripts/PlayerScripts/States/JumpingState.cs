using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumpingState : PlayerState
{
    public override string GiveName()
    {
        return "Jumping";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {

        if (self.VerticalState == Player.VState.grounded)
        {
            self.CanJumpIndex = 0;
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0) + calculate.addForce;
            if (body.velocity.x < 0.25f && body.velocity.x > -0.25f)
            {
                body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
            }
            if (body.velocity.x == 0)
            {
                self.SetState(new IdleState());
            }
        }

        if (MovementCheck(input.horizontalInput))
        {
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;

            self.SetState(new MovingState());
        }
    }

    public override bool StickToGround() => false;
}

