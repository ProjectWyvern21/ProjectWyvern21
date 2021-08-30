using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPause = false;
    public GameObject pauseMenuUI;
    public GameObject playerUI;
    //public GameObject miniMapUI;
    public GameObject inventoryUI;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryUI.activeInHierarchy == false)
        {
            Pause();
        }

    }

     public void Resume()
    {
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        //miniMapUI.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
        isPause = false;
    }

    public void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        //miniMapUI.SetActive(false);
        Time.timeScale = 0f;
        isPause = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
