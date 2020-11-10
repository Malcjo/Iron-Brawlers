using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NullState : PlayerState
{
    public override string GiveName()
    {
        return "Null";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool crouchInput, bool jumpInput)
    {

    }
}

