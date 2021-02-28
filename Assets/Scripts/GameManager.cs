using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
//using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net.Http.Headers;

public enum MenuLayer { Title, Main_Menu, Character_Select, Stage_Select, Settings, credits, GameScreen}
public class GameManager : MonoBehaviour
{
    MenuLayer currentScreen;
    MenuLayer PreviousLayer;
    public PlayerInputManager inputManager;
    [SerializeField] private Timer timer;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] GameObject Title, MenuGroup, MainMenu, CharacterSelect, GameUIGroup;
    [SerializeField] Button PlayButton;
    [SerializeField] GameObject mainCamera;
    [SerializeField] private GameObject eventSystem;
    public bool inGame = false;
    bool player1Ready, player2Ready;
    public Transform player1Spawn, player2Spawn;
    public int player1Rounds, player2Rounds;
    [SerializeField] private int sceneIndex;
    private CameraScript cameraScript;
    public GameObject uimodule;
    [SerializeField] private int leftBounds, rightBounds, belowBounds, highBounds;

    [SerializeField] GameObject player1Wins, player2Wins, player1Loses, bothLose;
    [SerializeField] private Slider player1UI, player2UI;

    [SerializeField] private GameObject player1Round1, player1Round2, player1Round3;
    [SerializeField] private GameObject player2Round1, player2Round2, player2Round3;

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
     * 10 = CONTROLS
     * 11 = AUDIO
     * 12 = GRAPHICS
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
        print(sceneIndex);
        sceneIndex = 0;
        if (eventSystem != null)
        {
            eventSystem = GameObject.FindWithTag("Event");
            inputManager = eventSystem.GetComponent<PlayerInputManager>();
        }

    }
    private void Start()
    {
        ConnectToGameManager(0);
        if (sceneIndex == 1)
        {
            Debug.Log("ResetMenu");
            ResetMenu();
        }
    }
    private void Update()
    {
        TrackPlayers();
        EnableJoiningManager();
        TrackPlayerRounds();
    }
    public void EnableEventSystemOBJ()
    {
        eventSystem.SetActive(true);

    }
    public void SelectPlayButton()
    {
        PlayButton.Select();
        eventSystem = GameObject.FindWithTag("Event");
        EventSystem system = eventSystem.gameObject.GetComponent<EventSystem>();
        system.firstSelectedGameObject = PlayButton.gameObject;
        system.SetSelectedGameObject(system.firstSelectedGameObject);
    }
    public void DisableEventSystemOBJ()
    {
        eventSystem.SetActive(false);
    }
    public void SetCameraScript(CameraScript script)
    {
        cameraScript = script;
    }
    public CameraScript GetCameraScript()
    {
        return cameraScript;
    }
    private void EnableMenuCanvas()
    {
        MenuGroup.SetActive(true);
        GameUIGroup.SetActive(false);
    }
    public void DisableMenuCanvas()
    {
        MenuGroup.SetActive(false);
        GameUIGroup.SetActive(true);
    }
    public void GetCameraObject(GameObject cam)
    {
        mainCamera = cam;
    }
    public Slider GetPlayer1UI()
    {
        return player1UI;
    }
    public Slider GetPlayer2UI()
    {
        return player2UI;
    }

    public void ConnectToGameManager(int CameraType)
    {
        Invoke("MoveGameManagerOutOfDontDestroy", 1);
        ConnectToCanvas(CameraType);
        Invoke("SetThisToDontDestroy", 1);
    }
    private void ConnectToCanvas(int CameraType)
    {
        GameObject camObj = mainCamera;
        DontDestroyOnLoad(camObj);
        Camera cam = camObj.gameObject.GetComponent<Camera>();
        Canvas canvas = mainCanvas.GetComponent<Canvas>();
        if (CameraType == 0)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
        else if (CameraType == 1)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        canvas.worldCamera = cam;
    }
    private void SetThisToDontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void MoveGameManagerOutOfDontDestroy()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    private void ResetMenu()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        CharacterSelect.SetActive(false);
        MainMenu.SetActive(true);
        Debug.Log("Conntect EventSystemObject");
        print(GameObject.FindGameObjectWithTag("Event"));
        eventSystem = GameObject.FindGameObjectWithTag("Event");
        inputManager = eventSystem.gameObject.GetComponent<PlayerInputManager>();
        eventSystem.gameObject.GetComponent<EventSystem>().firstSelectedGameObject = PlayButton.gameObject;
        PlayButton.Select();
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
    public int draws;
    public void TimerRunOut()
    {
        draws++;
        ResetPlayers();
    }
    private void TrackPlayers()
    {
        if(players.Count > 0)
        {
            if(draws == 3)
            {
                Destroy(players[0].gameObject);
                Destroy(players[1].gameObject);
                bothLose.SetActive(true);
                timer.pause = true;
                Invoke("TransisitonBackToMainMenu", 2);
            }
            TrackPlayer1();
            if (players.Count > 1)
            {
                TrackPlayer2();
            }
            if (player1Rounds == 3 || player2Rounds == 3)
            {
                if(players.Count > 1)
                {
                    if (player1Rounds > player2Rounds)
                    {
                        Debug.Log("Player 1 wins!");
                        player1Wins.SetActive(true);
                        Destroy(players[1].gameObject);
                    }
                    else if (player2Rounds < player1Rounds)
                    {
                        Debug.Log("Player 2 wins!");
                        player2Wins.SetActive(true);
                        Destroy(players[0].gameObject);
                    }
                }
                else
                {
                    Debug.Log("Player 1 Loses!");
                    player1Loses.SetActive(true);
                }
                Invoke("TransisitonBackToMainMenu", 2);
            }

        }
    }
    private void TransisitonBackToMainMenu()
    {
        player1Round1.SetActive(false);
        player1Round2.SetActive(false);
        player1Round3.SetActive(false);
        player2Round1.SetActive(false);
        player2Round2.SetActive(false);
        player2Round3.SetActive(false);
        player1Wins.SetActive(false);
        player2Wins.SetActive(false);
        player1Loses.SetActive(false);
        bothLose.SetActive(false);
        SetRoundsToZero();
        SceneManager.LoadScene(0);
        players.Clear();
        ChangeSceneIndex(1);
        EnableMenuCanvas();
        ResetMenu();
        sceneIndex = 1;
    }
    public void AddPlayerToList(GameObject player)
    {
        players.Add(player);
    }
    private void TrackPlayerRounds()
    {
        TrackPlayer1Rounds();
        TrackPlayer2Rounds();
    }
    private void TrackPlayer1Rounds()
    {
        switch (player1Rounds)
        {
            case 1:
                player1Round1.SetActive(true);
                break;
            case 2:
                player1Round2.SetActive(true);
                break;
            case 3:
                player1Round3.SetActive(true);
                break;
        }
    }
    private void TrackPlayer2Rounds()
    {
        switch (player2Rounds)
        {
            case 1:
                player2Round1.SetActive(true);
                break;
            case 2:
                player2Round2.SetActive(true);
                break;
            case 3:
                player2Round3.SetActive(true);
                break;
        }
    
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
