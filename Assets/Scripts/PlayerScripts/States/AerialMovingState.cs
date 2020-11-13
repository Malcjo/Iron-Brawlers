using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AerialMovingState : PlayerState
{
    public override string GiveName()
    {
        return "AerialMovingState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunAirborneMoveState();
        if (!MovementCheck(horizontalInput))
        {
            self.SetState(new AerialIdleState());
        }
        if (JumpingCheck(jumpInput) && MovementCheck(horizontalInput))
        {
            self.SetState(new JumpingState());
        }
        if (self.GetVerticalState() == Player.VState.grounded)
        {
            self.SetState(new MovingState());
        }
        if(MovementCheck(horizontalInput))
        {
            if(horizontalInput == self.facingDirection * -1)
            {
                Debug.Log("Back air");
            }
            if(horizontalInput == self.facingDirection)
            {
                Debug.Log("Forward air");
            }
        }
        if (AttackCheck(attackInput))
        {
            self.SetState(new AerialAttackState());
        }
    }
}

