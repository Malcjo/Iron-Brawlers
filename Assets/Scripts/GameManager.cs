using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player[] playersInScene;
    public TMP_Text player1Lives, player2Lives;


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

    private void Update()
    {
        if("Main Menu" == SceneManager.GetActiveScene().name)
        {
            return;
        }
        if(playersInScene[0].lives == 0 || playersInScene[1].lives == 0)
        {
            SceneManager.LoadScene(0);
        }
        player1Lives.text = ("player 1 Lives : " + playersInScene[0].lives);
        player2Lives.text = ("player 2 Lives : " + playersInScene[1].lives);
    }

    public void FindPlayers()
    {
        playersInScene = FindObjectsOfType<Player>();

        for (int i = 0; i < playersInScene.Length; i++)
        {
            playersInScene[i].GetComponent<Player>();
        }
    }

}
