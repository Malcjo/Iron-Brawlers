using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusyState : PlayerState
{
    public override string GiveName()
    {
        return "BusyState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {

    }
}


