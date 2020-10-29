using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player[] playersInScene;
    public TMP_Text player1Lives, player2Lives;
    public Image player1Image, player2Image;
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

        mainMenu = GetComponentInChildren<MainMenu>();
    }

    private void Update()
    {
        if("Main Menu" == SceneManager.GetActiveScene().name)
        {
            return;
        }
        if(playersInScene[0].lives == 0 || playersInScene[1].lives == 0)
        {
            mainMenu.GoToCharacterSelect();
        }
        player1Lives.text = ("player 1 Lives : " + playersInScene[0].lives);
        player2Lives.text = ("player 2 Lives : " + playersInScene[1].lives);
        if(playersInScene[0].characterType == 0)
        {
            player1Image.sprite = flowerBoi1;
        }
        else if (playersInScene[0].characterType == 1)
        {
            player1Image.sprite = flowerBoi2;
        }
        if(playersInScene[1].characterType == 0)
        {
            player2Image.sprite = flowerBoi1;
        }
        else if (playersInScene[1].characterType == 1)
        {
            player2Image.sprite = flowerBoi2;
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
    void PlayerNumberDetermine(int i)
    {
        if (i == 0)
        {
            playersInScene[i].playerNumber = PlayerIndex.Player1;
            playersInScene[i].characterType = player1CharacterIndex;
        }
        else if (i == 1)
        {
            playersInScene[i].playerNumber = PlayerIndex.Player2;
            playersInScene[i].characterType = player2CharacterIndex;
        }
    }

}
