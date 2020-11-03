﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    private ArmourCheck armourCheck;

    public GameObject[] meshRendering;

    public Material flowerBoiSkin1, flowerBoiSkin2;
    private int characterType;

    public float speed;
    public float jumpForce;
    public float weight;
    public float knockbackResistance;

    private void Awake()
    {
        armourCheck = GetComponent<ArmourCheck>();
        player = GetComponent<Player>();
    }
    private void Start()
    {
        characterType = player.characterType; 
        switch (characterType)
        {
            case 1:
                foreach (GameObject mesh in meshRendering)
                {
                    mesh.GetComponent<SkinnedMeshRenderer>().material = flowerBoiSkin1;
                }
                break;
            case 2:
                foreach (GameObject mesh in meshRendering)
                {
                    mesh.GetComponent<SkinnedMeshRenderer>().material = flowerBoiSkin2;
                }
                break;
        }
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
        if (player.currentJumpIndex == 0)
        {
            return player.SetVelocityY();
        }
        else if(player.currentJumpIndex > 0)
        {
            if (player.jumping == false && player.falling == false)
            {
                jumpForceValue = jumpForce - armourCheck.reduceJumpForce;
                return jumpForceValue;
            }
            else if (player.jumping == true || player.falling == true)
            {
                Debug.Log("Jump in air");
                jumpForceValue = (jumpForce + 2) - armourCheck.reduceJumpForce;
                return jumpForceValue;
            }
        }
        return 0;
    }
}
