using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    public GameObject PauseMenu;
    public static bool GameIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        if(PauseMenu)
        {
            this.PauseMenu.SetActive(false);
        } 
        else
        {
            // Debug.Log("No Pause Menu Given");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } 
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        this.PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        this.PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        // Debug.Log("Loading Menu...");
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        // Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
