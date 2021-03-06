﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleState : PlayerState
{
    public override string GiveName()
    {
        return "Idle";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        self.CanMove = true;
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
            Debug.Log("Crouching");
            //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            body.velocity = new Vector3(0, 0, 0) + calculate.addForce;
            self.SetState(new CrouchingState());
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
        if (AttackCheck(input.attackInput))
        {
            self.CanMove = false;
            body.velocity = new Vector3(0, body.velocity.y, 0);
            self.CanTurn = false;
            actions.JabCombo();
            self.SetState(new BusyState());
        }
        if (HeavyCheck(input.heavyInput))
        {
            self.CanMove = false;
            actions.Heavy();
            self.CanTurn = false;
            self.SetState(new BusyState());
            self.StopMovingCharacterOnXAxis();
        }
        if (BlockCheck(input.blockInput))
        {
            actions.Block();
            self.Blocking = true;
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            self.SetState(new BlockState());
        }
        if (!BlockCheck(input.blockInput))
        {
            self.Blocking = false;
        }

    }
}

