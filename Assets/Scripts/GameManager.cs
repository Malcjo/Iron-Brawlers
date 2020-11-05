using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private float boundsUp, boundsDown, boundsLeft, boundsRight;
    public Player[] playersInScene;
    MainMenu mainMenu;
    public int player1CharacterIndex, player2CharacterIndex;

    public Sprite flowerBoi1, flowerBoi2;

    private void Awake()
    {
        #region Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        #endregion   
    }

    private void Start()
    {
        boundsUp = 20;
        boundsDown = -20;
        boundsLeft = -20;
        boundsRight = 20;

        mainMenu = GetComponentInChildren<MainMenu>();
    }

    private void Update()
    {
        if("Main Menu" == SceneManager.GetActiveScene().name)
        {
            return;
        }
        for(int i = 0; i < playersInScene.Length; i++)
        {
            if (playersInScene[i].lives == 0)
            {
                mainMenu.GoToCharacterSelect();
            }
        }
    }

    public void FindPlayers()
    {
        playersInScene = FindObjectsOfType<Player>();

        for (int i = 0; i < playersInScene.Length; i++)
        {
            playersInScene[i].GetComponent<Player>();
            PlayerNumberDetermine(i);
        }
    }
    private void PlayerNumberDetermine(int i)
    {
        if (i == 0)
        {
            playersInScene[i].playerNumber = Player.PlayerIndex.Player1;
            playersInScene[i].characterType = player1CharacterIndex;
        }
        else if (i == 1)
        {
            playersInScene[i].playerNumber = Player.PlayerIndex.Player2;
            playersInScene[i].characterType = player2CharacterIndex;
        }
    }

    public static bool CheckIsInBounds(Vector3 position)
    {
        return position.x > instance.boundsRight || position.x < instance.boundsLeft || position.y > instance.boundsUp || position.y < instance.boundsDown;
    }

    public static Sprite GetSprite(Player.PlayerIndex playerIndex)
    {
        switch (playerIndex)
        {
            case Player.PlayerIndex.Player1: return instance.flowerBoi1;
            case Player.PlayerIndex.Player2: return instance.flowerBoi2;
        }
        return null;
    }
}
