using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandingState : PlayerState
{
    public override string GiveName()
    {
        return "LandingState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.InAir = false;
        self.CanMove = false;
        body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
        actions.JumpLanding();
    }
}

