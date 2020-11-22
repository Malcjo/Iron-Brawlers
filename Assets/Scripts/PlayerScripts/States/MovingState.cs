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
        self.CanTurn = true;
        body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
        actions.Running();
        if (!MovementCheck(input.horizontalInput))
        {
            self.SetState(new IdleState());
        }
        if (CrouchingCheck(input.crouchInput))
        {
            self.SetState(new CrouchingState());
        }
        if (JumpingCheck(input.jumpInput))
        {
            self.SetState(new JumpingState());
        }
        if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
        {
            self.StopMovingCharacter();
            self.SetState(new HeavyState());
        }
    }
}

