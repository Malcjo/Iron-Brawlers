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
    [SerializeField]
    private MainMenu mainMenu;
    public int player1CharacterIndex, player2CharacterIndex;

    public Sprite flowerBoi1, flowerBoi2;
    private bool ingame = false;

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
    }

    private void Update()
    {
        if("Main Menu" == SceneManager.GetActiveScene().name)
        {
            return;
        }
        if(ingame == true)
        {
            for (int i = 0; i < playersInScene.Length; i++)
            {
                if (playersInScene[i].lives == 0)
                {
                    mainMenu.GoToCharacterSelect();
                }
            }
        }
    }
    public void StartMatch()
    {
        FindPlayers();
        SetPlayersLivesToThree();
        InGame();
    }
    public void SetPlayersLivesToThree()
    {
        for (int i = 0; i < playersInScene.Length; i++)
        {
            playersInScene[i].lives = 3;
        }
    }
    public void InGame()
    {
        ingame = true;
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
    public GameObject[] playerCharacters;
    public void SetPlayerCharacter(int index, GameObject character)
    {
        switch (index)
        {
            case 1: playerCharacters[0] = character;
                break;
            case 2: playerCharacters[1] = character;
                break;
        }
    }
    private void PlayerNumberDetermine(int index)
    {
        if (index == 0)
        {
            playersInScene[index].playerNumber = Player.PlayerIndex.Player1;
            playersInScene[index].characterType = player1CharacterIndex;
        }
        else if (index == 1)
        {
            playersInScene[index].playerNumber = Player.PlayerIndex.Player2;
            playersInScene[index].characterType = player2CharacterIndex;
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
