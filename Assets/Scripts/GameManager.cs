using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerInputManager inputManager;
    [SerializeField] private EventSystem eventSystem;
    bool player1Ready, player2Ready;
    public Transform player1Spawn, player2Spawn;
    private int sceneIndex;
    /*
     * 0 = title
     * 1 = main meuu
     * 2 = Character select
     * 3 = stage select
     * 4 = settings
     * 5 = credtis
     * 6 = concept art
     * 7 = In game
     * 8 = Pause Screen
     * 9 = Victory screen
     */

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        sceneIndex = 0;
        if (eventSystem != null)
        {
            eventSystem = FindObjectOfType <EventSystem>();
        }
    }
    private void Update()
    {
        EnableJoiningManager();
    }
    public void SetPlayerSpawns(Transform _player1Spawn, Transform _player2Spawn)
    {
        player1Spawn = _player1Spawn;
        player2Spawn = _player2Spawn;
    }
    public void ReadyPlayer()
    {
        if (player1Ready == true)
        {
            player2Ready = true;
            //player.playerNumber = Player.PlayerIndex.Player2;
        }
        else
        {
            player1Ready = true;
            //player.playerNumber = Player.PlayerIndex.Player1;
        }
    }
    public bool GetPlayer1Ready()
    {
        return player1Ready;
    }
    public bool GetPlayer2Ready()
    {
        return player2Ready;
    }
    public void ResetPlayersReady()
    {
        player1Ready = false;
        player2Ready = false;
    }

    public void ChangeSceneIndex(int index)
    {
        sceneIndex = index;
    }
    private void EnableJoiningManager()
    {
        switch (sceneIndex)
        {
            case 0:
                foreach (Player ob in FindObjectsOfType(typeof(Player)))
                {
                    Destroy(ob);
                }
                inputManager.DisableJoining();
                break;
            case 1:
                inputManager.DisableJoining();
                break;
            case 2:
                inputManager.EnableJoining();
                break;
            case 3:
                inputManager.DisableJoining();
                break;
            case 4:
                inputManager.DisableJoining();
                break;
            case 5:
                inputManager.DisableJoining();
                break;
            case 6:
                inputManager.DisableJoining();
                break;
            case 7:
                inputManager.DisableJoining();
                break;
            case 8:
                inputManager.DisableJoining();
                break;
            case 9:
                inputManager.DisableJoining();
                break;
        }
    }

}
