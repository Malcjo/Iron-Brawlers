using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
//using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum MenuLayer { Title, Main_Menu, Character_Select, Stage_Select, Settings, credits, GameScreen}
public class GameManager : MonoBehaviour
{
    MenuLayer currentScreen;
    MenuLayer PreviousLayer;
    public PlayerInputManager inputManager;
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
        ConnectCameraToCanvas();
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
        //check the selection of input system ui
        //use event system get component to find it
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
    //need to take camera out of DontDestroyOnLoad to then attach camera to canvas and then bring it back
    //or jsut have a prefabed cavas on each scene, might have issue with getting the main buttons to select when transitioning through the script
    public void ConnectCameraToCanvas()
    {
        GameObject camObj = mainCamera;
        DontDestroyOnLoad(camObj);
        Camera cam = camObj.gameObject.GetComponent<Camera>();
        Canvas canvas = mainCanvas.GetComponent<Canvas>();
        canvas.worldCamera = cam;
        SceneManager.MoveGameObjectToScene(camObj.gameObject, SceneManager.GetActiveScene());
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
                players.Clear();
                ChangeSceneIndex(1);
                EnableMenuCanvas();
                ResetMenu();
                sceneIndex = 0;
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
