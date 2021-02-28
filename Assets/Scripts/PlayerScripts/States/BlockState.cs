using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockState : PlayerState
{
    public override string GiveName()
    {
        return "BlockState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        if (!BlockCheck(input.blockInput))
        {
            actions.StopBlock();
            self.SetState(new IdleState());
        }
    }
}


