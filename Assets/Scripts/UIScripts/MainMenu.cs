using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//public enum MenuLayer { Title, Main_Menu, Character_Select, Stage_Select, Settings, credits, }
public class MainMenu : MonoBehaviour
    
{
    [SerializeField] MenuLayer currentMenu;
    [SerializeField] MenuLayer previousMenu;
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

    [SerializeField] private OldGameManager gamemanager;
    [SerializeField] private PlayerSetupMenuController menuController;


    private void Awake()
    {
        gamemanager = GetComponentInParent<OldGameManager>();
    }
    private void Start()
    {
        currentMenu = MenuLayer.Title;
        inTitleScreen = true;
        playerSelected = 1;
        titleScreen.SetActive(true);
        gameUIGroup.SetActive(false);
        inGame = false;
        inTitleScreen = true;
        //GameManager.instance.DisableEventSystemOBJ();
    }

    private void Update()
    {
        if(inTitleScreen == true)
        {
            if (Input.anyKey)
            {
                if (currentMenu == MenuLayer.Title)
                {
                    currentMenu = MenuLayer.Main_Menu;
                    previousMenu = MenuLayer.Title;
                    //GameManager.instance.EnableEventSystemOBJ();
                }

                titleScreen.SetActive(false);
                mainMenuScreen.SetActive(true);
                GameManager.instance.SelectPlayButton();
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
    public void ChangePreviousMenu(string _previousMenu)
    {
        switch (_previousMenu)
        {
            case "Title":
                previousMenu = MenuLayer.Title;
                break;
            case "Main_Menu":
                previousMenu = MenuLayer.Main_Menu;
                break;
            case "Character_Select":
                previousMenu = MenuLayer.Character_Select;
                break;
            case "Stage_Select":
                previousMenu = MenuLayer.Stage_Select;
                break;
            case "Settings":
                previousMenu = MenuLayer.Settings;
                break;
            case "credits":
                previousMenu = MenuLayer.credits;
                break;
            default:
                previousMenu = MenuLayer.Title;
                break;
        }
    }
    public void ChangeMenu(string _CurrentMenu)
    {
        switch (_CurrentMenu)
        {
            case "Title":
                currentMenu = MenuLayer.Title;
            break;
            case "Main_Menu":
                currentMenu = MenuLayer.Main_Menu;
                break;
            case "Character_Select":
                currentMenu = MenuLayer.Character_Select;
                break;
            case "Stage_Select":
                currentMenu = MenuLayer.Stage_Select;
                break;
            case "Settings":
                currentMenu = MenuLayer.Settings;
                break;
            case "credits":
                currentMenu = MenuLayer.credits;
                break;
            default:
                currentMenu = MenuLayer.Title;
                break;

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
                OldGameManager.instance.SetPlayerCharacter(1, Character);
                playerSelected++;
                break;
            case 2:
                OldGameManager.instance.SetPlayerCharacter(2, Character);
                GotToStageSelect();
                break;
        }
    }
    //public void SelectCharacter1()
    //{
    //    if (player1Selected == true && player2Selected == false)
    //    {
    //        PlayerConfigurationManager.Instance.SetPlayerCharacter(0, 0);
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
