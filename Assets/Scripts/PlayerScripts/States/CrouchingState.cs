﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrouchingState : PlayerState
{
    public override string GiveName()
    {
        return "Crouching";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        actions.Crouching();
        if (!CrouchingCheck(input.crouchInput))
        {
            self.SetState(new IdleState());
        }
        if (AttackCheck(input.attackInput))
        {
            self.CanMove = false;
            self.CanTurn = false;
            actions.LegSweep();
            self.SetState(new BusyState());
        }
        if (ArmourBreakCheck(input.armourBreakInput))
        {
            self.SetState(new ArmourBreakState());
        }
    }
}

