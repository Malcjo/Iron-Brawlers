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
                break;
            case PlayerIndex.Player2:
                horizontalKeys = "P2Horizontal";
                jumpKey = KeyCode.Keypad5;
                jabKey = KeyCode.Keypad1;
                crouchKey = KeyCode.DownArrow;
                break;
        }

    }
}
