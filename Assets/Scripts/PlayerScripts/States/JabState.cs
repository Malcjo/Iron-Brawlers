using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JabState : PlayerState
{
    public override string GiveName()
    {
        return "JabState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        actions.JabCombo();

        self.SetState(new BusyState());
        Debug.Log("Jab");
    }
}

