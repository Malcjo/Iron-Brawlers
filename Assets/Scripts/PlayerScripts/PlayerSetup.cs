using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSetup : MonoBehaviour
{
    public int playerOneLayer;
    public int playerTwoLayer;

    public string horizontalKeys;
    public KeyCode jumpKey, attackKey, crouchKey, blockKey, armourKey;

    public int selfLayer, enemyLayer;

    public GameObject tipHitBox,midHitBox;
    public GameObject[] chestArmour;
    public GameObject[] legArmour;


    public GameObject[] meshRendering;

    public Material flowerBoiSkin1, flowerBoiSkin2;
    private int characterType;

    Player player;

    private Player.PlayerIndex playerNumber;


    void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        characterType = player.characterType;
        playerNumber = player.playerNumber;

        SetupSkin();
        SetupControls();
    }

    void SetupSkin()
    {
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

    void SetupControls()
    {
        switch(playerNumber)
        {
            case Player.PlayerIndex.Player1:
                horizontalKeys = "P1Horizontal";

                jumpKey = KeyCode.Y;
                attackKey = KeyCode.G;
                crouchKey = KeyCode.S;
                blockKey = KeyCode.J;
                armourKey = KeyCode.H;

                SetupLayers(playerOneLayer, playerTwoLayer);
                break;
            case Player.PlayerIndex.Player2:
                horizontalKeys = "P2Horizontal";

                jumpKey = KeyCode.Keypad5;
                attackKey = KeyCode.Keypad1;
                crouchKey = KeyCode.DownArrow;
                blockKey = KeyCode.Keypad3;
                armourKey = KeyCode.Keypad2;

                SetupLayers(playerTwoLayer, playerOneLayer);
                break;
        }

    }

    private void SetupLayers(int self, int other)
    {
        tipHitBox.layer = self;
        selfLayer = self;
        enemyLayer = other;
        ChangeArmourLayer(self);
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
