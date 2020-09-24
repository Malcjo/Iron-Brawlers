using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        titleScreen.SetActive(true);
        gameUIGroup.SetActive(false);
        inGame = false;
        

    }

    private void Update()
    {
        if (Input.anyKey)
        {
            titleScreen.SetActive(false);
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
    } 
}
