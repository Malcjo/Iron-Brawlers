using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerInputManager inputManager;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] GameObject MenuObject;
    [SerializeField] private EventSystem eventSystem;
    public bool inGame = false;
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
        TrackPlayers();
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
        if(players.Count > 0)
        {
            TrackPlayer1();
            if (players.Count > 1)
            {
                TrackPlayer2();
            }
            if (player1Rounds == 3 || player2Rounds == 3)
            {
                SetRoundsToZero();
                SceneManager.LoadScene(0);
            }
        }

    }
    public void AddPlayerToList(GameObject player)
    {
        players.Add(player);
    }
    private void TrackPlayer1()
    {
        if (players[0].gameObject.transform.position.y <= belowBounds)
        {
            player2Rounds++;
            ResetPlayers();
        }
    }
    private void TrackPlayer2()
    {
        if (players[1].gameObject.transform.position.y <= belowBounds)
        {
            player1Rounds++;
            ResetPlayers();
        }
    }
    private void ResetPlayers()
    {
        players[0].gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
        players[0].gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
        players[0].gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
        players[0].transform.position = player1Spawn.transform.position;
        if (players.Count > 1)
        {
            players[1].gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
            players[1].gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
            players[1].gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
            players[1].transform.position = player2Spawn.transform.position;
        }
    }
    private void SetRoundsToZero()
    {
        player1Rounds = 0;
        player2Rounds = 0;
    }

}
