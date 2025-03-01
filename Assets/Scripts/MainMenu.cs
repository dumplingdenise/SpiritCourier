using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
        
    }

    public void StartGame()
    {
/*        SceneManager.LoadScene("Play");*/
        SceneManager.LoadScene("Denise");
    }

    public void Awake()
    {
        int screenW = 1920;
        int screenH = 1080;
        bool isFullScreen = true;

        Screen.SetResolution(screenW, screenH, isFullScreen);
    }
}
