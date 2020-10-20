using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    private ArmourCheck armourCheck;

    public float speed;
    public float jumpForce;
    public float weight;
    public float knockbackResistance;

    private void Awake()
    {
        armourCheck = GetComponent<ArmourCheck>();
        player = GetComponent<Player>();
    }
    public float CharacterSpeed()
    {
        float characterSpeed = speed - armourCheck.armourReduceSpeed;
        if (player.hitStun == true)
        {
            characterSpeed *= 0 + (5 * Time.deltaTime);
        }
        return characterSpeed;
    }
    public float JumpForceCalculator()
    {
        float jumpForceValue;
        if (player.numberOfJumps == 0)
        {
            return player.SetVelocityY();
        }
        if (player.jumping == false)
        {
            jumpForceValue = jumpForce - armourCheck.reduceJumpForce;
            return jumpForceValue;
        }
        jumpForceValue = (jumpForce + 1) - armourCheck.reduceJumpForce;
        return jumpForceValue;
    }
}
