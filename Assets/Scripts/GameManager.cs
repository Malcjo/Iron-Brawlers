﻿using System.Collections;
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
    [SerializeField] private Timer timerScript;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] GameObject Title, MenuGroup, MainMenu, CharacterSelect, GameUIGroup;
    [SerializeField] Button PlayButton;
    [SerializeField] GameObject mainCamera;
    [SerializeField] private GameObject eventSystem;
    public bool inGame = false;
    [SerializeField] bool player1Ready;
    [SerializeField] bool player2Ready;
    public Transform player1Spawn, player2Spawn;
    public int player1Rounds, player2Rounds;
    [SerializeField] private int sceneIndex;
    private CameraScript cameraScript;
    public GameObject uimodule;
    [SerializeField] private int leftBounds, rightBounds, belowBounds, highBounds;

    [Header("In Game UI")]
    [SerializeField] GameObject player1Wins, player2Wins, player1Loses, bothLose;
    
    [SerializeField] private Slider player1UI, player2UI;

    [SerializeField] private GameObject player1Round1, player1Round2, player1Round3;
    [SerializeField] private GameObject player2Round1, player2Round2, player2Round3;

    [SerializeField] private GameObject player1Head, player1Chest, player1Legs;
    [SerializeField] private GameObject player2Head, player2Chest, player2Legs;



    [Header("Menu UI")]

    public GameObject player1SolAnimated, player1SolAltAnimated, player1GoblinAnimated, player1GoblinAltAnimated;
    public GameObject player2SolAnimated, player2SolAltAnimated, player2GoblinAnimated, player2GoblinAltAnimated;
    public GameObject player1Character1PortraitPuck, player1Character2PortraitPuck;
    public GameObject player2Character1PortraitPuck, player2Character2PortraitPuck;
    public GameObject player1CharacterPuck, player2CharacterPuck;

    public GameObject player1Character1Selected, player1Character2Selected;
    public GameObject player2Character1Selected, player2Character2Selected;
    public GameObject character1ButtonSelected, character2ButtonSelected;

    public bool Character1BeenPicked = true;
    public bool Character2BeenPicked = true;
    public bool canJoin = false;

    public bool StartGame = false;
    public bool levelSelect = false;
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
        canJoin = false;
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
        TrackPlayersArmour();
    }
    public void ReadyPlayer(int i)
    {
        switch (i)
        {
            case 1:
                player1Ready = true;
                break;
            case 2:
                player2Ready = true;
                break;
        }
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

    public void ChangeCharacterModelIfSameIsChosen(int index, GameObject character, float currentCharacter)
    {
        GameObject _characterSelect;
        _characterSelect = CharacterSelect;
        if (index == 1 - 1)
        {
            BindToPlayer bind;

            bind = _characterSelect.GetComponent<BindToPlayer>();
            bind.players[1].GetComponent<PlayerInputHandler>().SwitchModel(character, currentCharacter);
        }
        else if(index == 2 - 1)
        {
            BindToPlayer bind;

            bind = _characterSelect.GetComponent<BindToPlayer>();
            bind.players[0].GetComponent<PlayerInputHandler>().SwitchModel(character, currentCharacter);
        }
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

        EnabledJoining();
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

    public void SwitchOnCharacterSelect()
    {
        Invoke("CharacterSelectTransition", 0.2f);
    }
    private void CharacterSelectTransition()
    {
        MainMenu.SetActive(false);
        CharacterSelect.SetActive(true);
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
            Debug.Log("Player 2 set ready");
            player2Ready = true;
            player1SolAltAnimated.SetActive(true);
            player2CharacterPuck.SetActive(true);
            player1Character2PortraitPuck.SetActive(false);
        }
        else
        {
            player1Ready = true;
            player1SolAnimated.SetActive(true);
            player1CharacterPuck.SetActive(true);
            player1Character1PortraitPuck.SetActive(false);
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
        player1SolAnimated.SetActive(false);
        player1SolAltAnimated.SetActive(false);
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
                //inputManager.EnableJoining();
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
    public void EnabledJoining()
    {
        StartCoroutine(DelayedEnabled());
    }
    public void DisableJoining()
    {
        inputManager.DisableJoining();
    }
    IEnumerator DelayedEnabled()
    {
        yield return new WaitForSeconds(0.5f);
        canJoin = true;
        inputManager.EnableJoining();
    }

    public int draws;
    public void TimerRunOut()
    {
        draws++;
        ResetPlayers();
    }
    [SerializeField] private float maxTimer;
    public void SetMaxTimer(float max)
    {
        maxTimer = max;
    }
    private void TrackPlayersArmour()
    {
        TrackPlayer1Armour();
        TrackPlayer2Armour();
    }
    private void TrackPlayer1Armour()
    {
        if(players.Count >= 1)
        {
            ArmourCheck playerArmour = players[0].GetComponent<ArmourCheck>();
            if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.armour)
            {
                player1Head.SetActive(true);
            }
            else if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.none)
            {
                player1Head.SetActive(false);
            }

            if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player1Chest.SetActive(true);
            }
            else if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player1Chest.SetActive(false);
            }

            if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player1Legs.SetActive(true);
            }
            else if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player1Legs.SetActive(false);
            }
        }
    }

    private void TrackPlayer2Armour()
    {
        if(players.Count > 1)
        {
            ArmourCheck playerArmour = players[1].GetComponent<ArmourCheck>();
            if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.armour)
            {
                player2Head.SetActive(true);
            }
            else if (playerArmour.HeadArmourCondiditon == ArmourCheck.ArmourCondition.none)
            {
                player2Head.SetActive(false);
            }

            if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player2Chest.SetActive(true);
            }
            else if (playerArmour.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player2Chest.SetActive(false);
            }

            if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                player2Legs.SetActive(true);
            }
            else if (playerArmour.LegArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                player2Legs.SetActive(false);
            }
        }
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
                timerScript.pause = true;
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
                    else if (player2Rounds > player1Rounds)
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
    public int LevelIDNumber;
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
        timerScript.ResetTimer();
        players[0].gameObject.GetComponent<Player>().SetJumpIndexTo1();
        players[0].gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
        players[0].gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
        players[0].gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
        players[0].transform.position = player1Spawn.transform.position;
        if (players.Count > 1)
        {
            players[1].gameObject.GetComponent<Player>().SetJumpIndexTo1();
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
