using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Player player;

    public string horizontalKeys;
    public KeyCode jumpKey;
    public KeyCode jabKey;

    void Start()
    {
        player = GetComponent<Player>();
    }


    void Update()
    {
        CheckControl();
    }
    void CheckControl()
    {
        switch(player.PlayerNumber)
        {
            case Player.PlayerIndex.Player1:
                horizontalKeys = "P1Horizontal";
                jumpKey = KeyCode.Y;
                jabKey = KeyCode.G;
                break;
            case Player.PlayerIndex.Player2:
                horizontalKeys = "P2Horizontal";
                jumpKey = KeyCode.Keypad5;
                jabKey = KeyCode.Keypad1;
                break;
        }

    }
}
