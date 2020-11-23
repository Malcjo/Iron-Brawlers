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

        if(self.CanMove == true)
        {
            if(self.VerticalState == Player.VState.grounded)
            {
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
            }
            else
            {
                body.velocity = new Vector3(input.horizontalInput * (calculate.characterSpeed / 1.5f), body.velocity.y, 0) + calculate.addForce;
            }
        }
        else
        {
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
        }

    }
}


