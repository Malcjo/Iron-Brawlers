using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BindToPlayer : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    [SerializeField] private PlayerJoinHandler join = null;

    public GameObject events = null;
    
    Scene currentScene;
    Scene menuScene;

    public int playerIndex;

    private void OnEnable()
    {
        menuScene = SceneManager.GetSceneByBuildIndex(0);
        currentScene = SceneManager.GetActiveScene();

        if (currentScene != SceneManager.GetActiveScene())
        {
            currentScene = SceneManager.GetActiveScene();
        }

        if (currentScene == menuScene)
        {
            join.SetPlayerBind(this);
            foreach (GameObject obj in players)
            {
                Destroy(obj);
            }
            players.Clear();
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            LevelSelectNumber = 1;
            SetLevelSelectedNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            LevelSelectNumber = 2;
            SetLevelSelectedNumber();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameManager.instance.StartGame = true;
        }
        if (SceneManager.GetActiveScene() == menuScene)
        {
            if (GameManager.instance.GetPlayer1Ready() == true && GameManager.instance.GetPlayer2Ready() == true)
            {
                GameManager.instance.levelSelect = true;
                if(GameManager.instance.StartGame == true)
                {
                    StartGame();
                }
            }
        }
    }
    private void SetLevelSelectedNumber()
    {
        GameManager.instance.LevelIDNumber = LevelSelectNumber;
    }
    [Range(1,2)]
    [SerializeField] private int LevelSelectNumber = 1;
    private void StartGame()
    {
        StartCoroutine(DelayStartGame());
    }
    IEnumerator DelayStartGame()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.DisableJoining();
        GameManager.instance.ResetPlayersReady();
        GameManager.instance.DisableMenuCanvas();
        SceneManager.LoadScene(LevelSelectNumber);
        GameManager.instance.ConnectToGameManager(1);
        GameManager.instance.inGame = true;
    }
    private void CheckIfPlayersAreReady()
    {

    }
    public void JoinGame(PlayerInput input)
    {
        if(this.gameObject.tag == "Joining")
        {
            if(input.playerIndex == 1 - 1)
            {
                GameManager.instance.player1Character1PortraitPuck.SetActive(true);
            }
            else if (input.playerIndex == 2 - 1)
            {
                GameManager.instance.player2Character1PortraitPuck.SetActive(true);
            }
            players.Add(input.gameObject);
            input.gameObject.GetComponent<PlayerInputHandler>().SetInput(input);
            playerIndex = players.Count;
            input.gameObject.GetComponent<PlayerInputHandler>().SetPlayerNumber(GameManager.instance.inputManager);
            input.gameObject.GetComponent<PlayerInputHandler>().PlayerIndex = playerIndex;
            //input.gameObject.GetComponent<PlayerInputHandler>().canAct = true;

            DontDestroyOnLoad(input.gameObject);
        }
    }
    //public void ReadyPlayer()
    //{
    //    GameManager.instance.ReadyPlayer();
    //}
}
