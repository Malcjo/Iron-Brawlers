using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
    
{
    public GameObject titleScreen;
    public GameObject mainMenuScreen;    
    public GameObject menuGroup;
    public GameObject pauseMenuGroup;
    public GameObject pauseMenu;
    public GameObject gameUIGroup;
    public GameObject settingsMenu;
    public GameObject characterMenu;
    public GameObject confirmMenu;
    public GameObject stageMenu;
    [SerializeField]
    private bool canPause;
    [SerializeField]
    private bool isPaused;
    [SerializeField]
    private bool inGame;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button Stage1Button;
    private bool inTitleScreen;
    [Range(1,2)]
    [SerializeField] private int playerSelected;

    [SerializeField] private GameManager gamemanager;
    [SerializeField] private PlayerSetupMenuController menuController;


    private void Awake()
    {
        gamemanager = GetComponentInParent<GameManager>();
    }
    private void Start()
    {
        inTitleScreen = true;
        playerSelected = 1;
        titleScreen.SetActive(true);
        gameUIGroup.SetActive(false);
        inGame = false;
    }

    private void Update()
    {
        if(inTitleScreen == true)
        {
            if (Input.anyKey)
            {
                titleScreen.SetActive(false);
                menuController.ChangedSelectedButton(PlayButton);
                inTitleScreen = false;
            }
        }


        if(inGame == false)
        {
            return;
        }


        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused == false)
                {
                    isPaused = true;
                    Time.timeScale = 0;
                    pauseMenuGroup.SetActive(true);
                    pauseMenu.SetActive(true);
                    gameUIGroup.SetActive(false);
                }
                else if (isPaused == true)
                {
                    Time.timeScale = 1;
                    gameUIGroup.SetActive(true);
                    pauseMenuGroup.SetActive(false);
                    menuGroup.SetActive(false);
                    settingsMenu.SetActive(false);
                    characterMenu.SetActive(false);
                    stageMenu.SetActive(false);
                    confirmMenu.SetActive(false);
                    isPaused = false;
                }
            }
        }
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadLevel(int level)
    {
        inGame = true;
        menuGroup.SetActive(false);
        gameUIGroup.SetActive(true);
        SceneManager.LoadScene(level);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuGroup.SetActive(false);  
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        inGame = false;
        gameUIGroup.SetActive(false);
        pauseMenuGroup.SetActive(false);
        menuGroup.SetActive(true);
        confirmMenu.SetActive(false);
        mainMenuScreen.SetActive(true);

    } 

    public void GoToCharacterSelect()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        inGame = false;
        gameUIGroup.SetActive(false);
        pauseMenuGroup.SetActive(false);
        menuGroup.SetActive(true);
        confirmMenu.SetActive(false);
        mainMenuScreen.SetActive(false);
        characterMenu.SetActive(true);
    }
    public void SelectCharacter(GameObject Character)
    {
        switch (playerSelected)
        {
            case 1:
                GameManager.instance.SetPlayerCharacter(1, Character);
                playerSelected++;
                break;
            case 2:
                GameManager.instance.SetPlayerCharacter(2, Character);
                GotToStageSelect();
                break;
        }
    }
    //public void SelectCharacter1()
    //{
    //    if (player1Selected == true && player2Selected == false)
    //    {
    //        PlayerConfigurationManager.Instance.SetPlayerCharacter(0,0);
    //        gamemanager.player1CharacterIndex = 1;
    //        player1Selected = false;
    //        player2Selected = true;
    //    }
    //    else if (player1Selected == false && player2Selected == true)
    //    {
    //        PlayerConfigurationManager.Instance.SetPlayerCharacter(0, 1);
    //        gamemanager.player2CharacterIndex = 1;
    //        player1Selected = true;
    //        player2Selected = false;
    //        GotToStageSelect();
    //    }
    //}

    //public void SelectCharacter2()
    //{
    //    if (player1Selected == true && player2Selected == false)
    //    {
    //        PlayerConfigurationManager.Instance.SetPlayerCharacter(1, 0);
    //        gamemanager.player1CharacterIndex = 2;
    //        player1Selected = false;
    //        player2Selected = true;

    //    }
    //    else if (player1Selected == false && player2Selected == true)
    //    {
    //        PlayerConfigurationManager.Instance.SetPlayerCharacter(1, 1);
    //        gamemanager.player2CharacterIndex = 2;
    //        player1Selected = true;
    //        player2Selected = false;
    //        GotToStageSelect();
    //    }
    //}
    void GotToStageSelect()
    {
        menuController.ChangedSelectedButton(Stage1Button);
        characterMenu.SetActive(false);
        stageMenu.SetActive(true);
    }
}
