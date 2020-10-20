using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public string horizontalKeys;
    public KeyCode jumpKey, jabKey, crouchKey, blockKey, armourKey;

    public int playersLayer, opponentLayer;
    //player 1 = layer 8
    //player 2 = layer 9

    public GameObject tipHitBox,midHitBox;
    public GameObject[] chestArmour;
    public GameObject[] legArmour;

    public PlayerIndex playerNumber;
    public enum PlayerIndex { Player1, Player2 };

    void Awake()
    {
        CheckControl();
    }

    void CheckControl()
    {
        switch(playerNumber)
        {
            case PlayerIndex.Player1:
                horizontalKeys = "P1Horizontal";

                jumpKey = KeyCode.Y;
                jabKey = KeyCode.G;
                crouchKey = KeyCode.S;
                blockKey = KeyCode.J;
                armourKey = KeyCode.H;


                tipHitBox.layer = 8;
                playersLayer = 8;
                opponentLayer = 9;
                ChangeArmourLayer(8);

                break;
            case PlayerIndex.Player2:
                horizontalKeys = "P2Horizontal";

                jumpKey = KeyCode.Keypad5;
                jabKey = KeyCode.Keypad1;
                crouchKey = KeyCode.DownArrow;
                blockKey = KeyCode.Keypad3;
                armourKey = KeyCode.Keypad2;

                tipHitBox.layer = 9;
                playersLayer = 9;
                opponentLayer = 8;
                ChangeArmourLayer(9);
                break;
        }

    }
    void ChangeArmourLayer(int layer)
    {
        for (int i = 0; i < chestArmour.Length; i++)
        {
            chestArmour[i].layer = layer;
        }
        for (int i = 0; i < legArmour.Length; i++)
        {
            legArmour[i].layer = layer;
        }
    }
}
