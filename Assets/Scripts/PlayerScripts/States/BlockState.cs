using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockState : PlayerState
{
    public override string GiveName()
    {
        return "BlockState";
    }
    public override void RunState(Player self, float horizontalInput, bool attackInput, bool jumpInput, bool crouchInput, bool armourBreakInput, bool blockInput)
    {
        self.RunBlock();
        if (!BlockCheck(blockInput))
        {
            self.SetState(new IdleState());
        }
    }
}

