using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockState : PlayerState
{
    public override string GiveName()
    {
        return "BlockState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        if (!BlockCheck(input.blockInput))
        {
            actions.StopBlock();
        }
    }
}


