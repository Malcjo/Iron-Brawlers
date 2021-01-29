using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovingState : PlayerState
{
    public override string GiveName()
    {
        return "Moving";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        if(self.VerticalState == Player.VState.grounded)
        {
            actions.Running();
        }
        else
        {
            if(self.VerticalState == Player.VState.jumping)
            {
                actions.Jumping();
            }
            else 
            { 
                actions.Falling(); 
            }
        }
        if (MovementCheck(input.horizontalInput))
        {
            body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
            body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
        }
        if (!MovementCheck(input.horizontalInput))
        {
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0) + calculate.addForce;
            if(body.velocity.x < 0.25f && body.velocity.x > -0.25f)
            {
                body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
            }
            if(body.velocity.x == 0)
            {
                self.SetState(new IdleState());
            }
        }
        if (CrouchingCheck(input.crouchInput))
        {
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            actions.Crouching();
            self.SetState(new CrouchingState());
        }
        if (JumpingCheck(input.jumpInput))
        {
            self.SetState(new JumpingState());
        }
        if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
        {
            self.SetState(new HeavyState());
            self.StopMovingCharacterOnXAxis();
        }
    }
}

