﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmourBreakState : PlayerState
{
    public override string GiveName()
    {
        return "ArmourBreakState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        actions.ArmourBreak();
        self.SetState(new BusyState());
    }
}

