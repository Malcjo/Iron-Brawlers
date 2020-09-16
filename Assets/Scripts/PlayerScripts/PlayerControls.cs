using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public string horizontalKeys;
    public KeyCode jumpKey, jabKey, crouchKey;

    public int playersLayer;
    //player 1 = layer 8
    //player 2 = layer 9

    public GameObject hitbox;
    public Player player;

    public PlayerIndex PlayerNumber;
    public enum PlayerIndex { Player1, Player2 };

    void Awake()
    {
        CheckControl();
    }

    void CheckControl()
    {
        switch(PlayerNumber)
        {
            case PlayerIndex.Player1:
                horizontalKeys = "P1Horizontal";
                jumpKey = KeyCode.Y;
                jabKey = KeyCode.G;
                crouchKey = KeyCode.S;
                hitbox.layer = 8;
                playersLayer = 8;

                break;
            case PlayerIndex.Player2:
                horizontalKeys = "P2Horizontal";
                jumpKey = KeyCode.Keypad5;
                jabKey = KeyCode.Keypad1;
                crouchKey = KeyCode.DownArrow;
                hitbox.layer = 9;
                playersLayer = 9;
                break;
        }

    }
}
