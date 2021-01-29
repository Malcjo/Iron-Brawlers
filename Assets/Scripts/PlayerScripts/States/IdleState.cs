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
        actions.Idle();
        if (MovementCheck(input.horizontalInput))
        {
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;

            self.SetState(new MovingState());
        }
        if (CrouchingCheck(input.crouchInput))
        {
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            actions.Crouching();
            self.SetState(new CrouchingState());
        }
        if (JumpingCheck(input.jumpInput))
        {
            if (self.CanJumpIndex < self.GetMaxJumps())
            {
                body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z)) + calculate.addForce;
                self.AddOneToJumpIndex();
                self.SetState(new JumpingState());
            }
        }
        if (AttackCheck(input.attackInput))
        {
            self.CanMove = false;
            body.velocity = new Vector3(0, body.velocity.y, 0);
            self.CanTurn = false;
            actions.JabCombo();

            self.SetState(new BusyState());
            Debug.Log("Jab");
        }
        if (BlockCheck(input.blockInput))
        {
            //self.SetState(new BusyState());
        }
        /*
         * idle state doing nothing
         * moving state running or moving in the air
         * airborne state in the air
         * crouching state crouching
         * busy state attacking, blocking or doing a special move, intro, knockdown, victory
         */

    }
}

