using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Player player;

    public string horizontalKeys;
    public KeyCode jumpKey;
    public KeyCode jabKey;
    public KeyCode crouchKey;
    public int oppositePlayersLayer;
    public int playersLayer;

    public GameObject hitbox;

    //player 1 = layer 8
    //player 2 = layer 9

    public PlayerIndex PlayerNumber;
    public enum PlayerIndex { Player1, Player2 };

    void Awake()
    {
        CheckControl();
        //this.gameObject.layer = playersLayer;
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
