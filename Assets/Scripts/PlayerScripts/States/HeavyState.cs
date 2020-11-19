using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeavyState : PlayerState
{
    public override string GiveName()
    {
        return "HeavyState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        actions.Heavy();
        self.CanTurn = false;
        self.SetState(new BusyState());
        Debug.Log("Heavy");
    }
}

