using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AerialMovingState : PlayerState
{
    public override string GiveName()
    {
        return "AerialMovingState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.CanTurn = false;
        self.InAir = true;
        self.CheckVerticalState();
        if (self.GetCanAirMove() == false)
        {
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0,calculate.friction), body.velocity.y, 0);
            return;
        }
        else if (self.GetCanAirMove() == true)
        {
            body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
        }
        if (!MovementCheck(input.horizontalInput))
        {
            self.SetState(new AerialIdleState());
        }
        if (JumpingCheck(input.jumpInput) && MovementCheck(input.horizontalInput))
        {
            self.SetState(new JumpingState());
        }
        if (self.VerticalState == Player.VState.grounded)
        {
            self.SetState(new MovingState());
        }
        if(MovementCheck(input.horizontalInput))
        {
            if(input.horizontalInput == self.facingDirection * -1)
            {
                Debug.Log("Back air");
            }
            if(input.horizontalInput == self.facingDirection)
            {
                Debug.Log("Forward air");
            }
        }
        if (AttackCheck(input.attackInput))
        {
            self.SetState(new AerialAttackState());
        }
        if (ArmourBreakCheck(input.armourBreakInput) && CrouchingCheck(input.crouchInput))
        {
            self.SetState(new ArmourBreakState());
        }

    }
}

