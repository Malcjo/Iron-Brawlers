using System.Collections;
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

        if (self.VerticalState == Player.VState.grounded)
        {

            actions.Running();

            if (MovementCheck(input.horizontalInput))
            {
                self.PlayRunningParticle();

                self.CanMove = true;
                self.CanTurn = true;
                //body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
            }

            if (!MovementCheck(input.horizontalInput))
            {
                self.StopRunningParticle();
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

            if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
            {
                self.CanMove = false;
                actions.Heavy();
                self.CanTurn = false;
                self.SetState(new BusyState());
                self.StopMovingCharacterOnXAxis();
            }

            if (AttackCheck(input.attackInput) && !MovementCheck(input.horizontalInput))
            {
                self.CanMove = false;
                body.velocity = new Vector3(0, body.velocity.y, 0);
                self.CanTurn = false;
                actions.JabCombo();
                self.SetState(new BusyState());
            }

            if (CrouchingCheck(input.crouchInput))
            {
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                actions.Crouching();
                self.SetState(new CrouchingState());
            }
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
            if(self.GetCanAirMove() == true)
            {
                if (MovementCheck(input.horizontalInput))
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
                    body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
                    if (input.horizontalInput == self.GetFacingDirection())
                    {
                        if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
                        {
                            actions.AerialAttack();
                            self.CanTurn = false;
                            self.WasAttacking = true;
                            self.SetState(new BusyState());
                        }
                    }
                    if (input.horizontalInput == self.GetFacingDirection() * -1)
                    {
                        if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
                        {
                            actions.AerialAttack();
                            self.CanTurn = false;
                            self.WasAttacking = true;
                            self.SetState(new BusyState());
                        }
                    }
                }
                if (JumpingCheck(input.jumpInput))
                {
                    if(self.canDoubleJump == true)
                    {
                        self.canDoubleJump = false;
                        self.CanTurn = false;
                        self.InAir = true;
                        body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z)) + calculate.addForce;
                        self.JumpingOrFallingAnimations();
                        self.AddOneToJumpIndex();
                        Debug.Log("DoubleJump");
                        self.SpawnDoubleJumpParticles();
                        self.SetState(new JumpingState());
                    }

                }
            }

            if (!MovementCheck(input.horizontalInput))
            {
                self.CanMove = true;
                self.CanTurn = false;
                self.InAir = true;
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0) + calculate.addForce;
                if (body.velocity.x < 0.25f && body.velocity.x > -0.25f)
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
                }
                if (body.velocity.x == 0)
                {
                    self.SetState(new JumpingState());
                }
            }
        }

        if (JumpingCheck(input.jumpInput))
        {
            if (self.CanJumpIndex < self.GetMaxJumps())
            {
                self.CanTurn = false;
                self.InAir = true;
                body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z)) + calculate.addForce;
                self.JumpingOrFallingAnimations();
                self.AddOneToJumpIndex();
                self.SetState(new JumpingState());
            }
        }

    }
}

