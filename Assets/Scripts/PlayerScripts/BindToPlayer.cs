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
        if (SceneManager.GetActiveScene() == menuScene)
        {
            if (GameManager.instance.GetPlayer1Ready() == true && GameManager.instance.GetPlayer2Ready() == true)
            {
                GameManager.instance.ResetPlayersReady();
                GameManager.instance.DisableMenuCanvas();
                SceneManager.LoadScene(1);
                GameManager.instance.ConnectToGameManager(1);
                GameManager.instance.inGame = true;
            }
        }
    }
    private void CheckIfPlayersAreReady()
    {

    }
    public void JoinGame(PlayerInput input)
    {
        players.Add(input.gameObject);
        input.gameObject.GetComponent<PlayerInputHandler>().SetPlayerNumber(GameManager.instance.inputManager);
        input.gameObject.GetComponent<PlayerInputHandler>().SetInput(input);
        DontDestroyOnLoad(input.gameObject);
    }
    public void ReadyPlayer()
    {
        GameManager.instance.ReadyPlayer();
    }
}
