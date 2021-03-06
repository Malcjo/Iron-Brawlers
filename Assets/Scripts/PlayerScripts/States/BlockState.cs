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
        body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
        if (!BlockCheck(input.blockInput))
        {
            self.Blocking = false;
            actions.StopBlock();
        }
        if (JumpingCheck(input.jumpInput))
        {
            if (self.CanJumpIndex < self.GetMaxJumps())
            {
                self.CanTurn = false;
                self.InAir = true;
                body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z)) + calculate.addForce;
                self.JumpingOrFallingAnimations();
                self.AddOneToJumpIndex();
                self.SetState(new JumpingState());
                self.Blocking = false;
                actions.StopBlock();
            }
        }
    }
}


