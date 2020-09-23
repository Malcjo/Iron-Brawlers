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
    public GameObject gameUIGroup;

    private void Start()
    {
        titleScreen.SetActive(true);
        gameUIGroup.SetActive(false);
             
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            titleScreen.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
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
        menuGroup.SetActive(false);
        gameUIGroup.SetActive(true);
        SceneManager.LoadScene(level);
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuGroup.SetActive(true);
        gameUIGroup.SetActive(false);
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
    }
    
}
