using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitStun_Normal_State : PlayerState
{
    public override string GiveName()
    {
        return "HitStun_Normal_State";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, InputState input, Calculating calculate)
    {
        self.CanMove = false;
        self.CanTurn = false;
        body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0) + calculate.addForce;
        actions.Idle();
    }
}

