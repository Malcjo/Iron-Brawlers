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
        else
        {
            if (AttackCheck(input.attackInput))
            {
                actions.AerialAttack();
                self.CanTurn = false;
                self.WasAttacking = true;
                self.SetState(new BusyState());
            }
            if (AttackCheck(input.attackInput) && (self.GetFacingDirection() > 0 || self.GetFacingDirection() < 0))
            {
                actions.AerialAttack();
                self.CanTurn = false;
                self.WasAttacking = true;
                self.SetState(new BusyState());
            }
            if (self.VerticalState == Player.VState.jumping)
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
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;

            self.SetState(new MovingState());
        }

        if (JumpingCheck(input.jumpInput))
        {
            if (self.CanJumpIndex < self.GetMaxJumps())
            {
                if(self.canDoubleJump == true)
                {
                    self.canDoubleJump = false;
                    self.CanTurn = false;
                    self.InAir = true;
                    body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce + 2, body.velocity.z)) + calculate.addForce;
                    self.JumpingOrFallingAnimations();
                    self.AddOneToJumpIndex();
                    Debug.Log("DoubleJump");
                    self.SpawnDoubleJumpParticles();
                    self.SetState(new JumpingState());
                }

            }
        }

        if (AttackCheck(input.attackInput))
        {
            actions.AerialAttack();
            self.CanTurn = false;
            self.WasAttacking = true;
            self.SetState(new BusyState());
        }

    }

    public override bool StickToGround() => false;
}

