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
        self.RunAirborneMoveState();
        if (!MovementCheck(input.horizontalInput))
        {
            self.SetState(new AerialIdleState());
        }
        if (JumpingCheck(input.jumpInput) && MovementCheck(input.horizontalInput))
        {
            self.SetState(new JumpingState());
        }
        if (self.GetVerticalState() == Player.VState.grounded)
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
    }
}

