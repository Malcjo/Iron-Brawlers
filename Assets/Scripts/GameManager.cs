using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerInputManager inputManager;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField] GameObject MenuObject;
    [SerializeField] private EventSystem eventSystem;

    bool player1Ready, player2Ready;
    public Transform player1Spawn, player2Spawn;
    public int player1Rounds, player2Rounds;
    private int sceneIndex;

    [SerializeField] private int leftBounds, rightBounds, belowBounds, highBounds;

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
    public void EnableMenuCanvas()
    {
        MenuObject.SetActive(true);
    }
    public void DisableMenuCanvas()
    {
        MenuObject.SetActive(false);
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
        }
        else
        {
            player1Ready = true;
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
                belowBounds = 0;
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
    private void TrackPlayers()
    {
        if (player1Rounds == 3 || player2Rounds == 3)
        {
            SetRoundsToZero();
            SceneManager.LoadScene(0);
        }
    }
    private void GetAllPlayersInScene()
    {
        foreach (Player obj in FindObjectsOfType(typeof(Player)))
        {
            players.Add(obj.gameObject);
        }
    }
    private void TrackPlayer1()
    {
        if (players[1].transform.position.y <= belowBounds)
        {
            player2Rounds++;
        }

    }
    private void ResetPlayers()
    {
    }
    private void SetRoundsToZero()
    {
        player1Rounds = 0;
        player2Rounds = 0;
    }

}
