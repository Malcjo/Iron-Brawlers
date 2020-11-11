﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrouchingState : PlayerState
{
    public override string GiveName()
    {
        return "Crouching";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput)
    {
        self.RunCrouchingState();
        if (!CrouchingCheck(crouchInput))
        {
            self.SetState(new IdleState());
        }
    }
}

