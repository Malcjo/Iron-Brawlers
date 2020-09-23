using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
    
{
    public GameObject titleScreen;
    public GameObject mainMenuScreen;    
    public GameObject menuGroup;
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
    
}
