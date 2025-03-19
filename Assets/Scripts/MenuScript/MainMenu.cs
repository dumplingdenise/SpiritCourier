using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject settingPanel;  // Settings UI Panel
    public Button settingButton;     // Settings Button
    public Button closeButton;       // Close Button

    public GameObject controlPanel;  // Control Panel
    public Button controlButton;     // Control Button
    public GameObject audioPanel;    // Audio Panel
    public Button audioButton;       // Audio Button
    public GameObject pausePanel;    // Pause Panel
    public Button pauseButton;       // Pause Button

    private Button activeButton; //track currently pressed button
    private void Awake()
    {
        int screenW = 1920;
        int screenH = 1080;
        bool isFullScreen = true;
        Screen.SetResolution(screenW, screenH, isFullScreen);

        // Ensure all panels are hidden at the start
        settingPanel.SetActive(false);
        controlPanel.SetActive(false);
        audioPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Start()
    {
        // Assign button click events
        settingButton.onClick.AddListener(OpenSettings);
        closeButton.onClick.AddListener(CloseSettings);

        controlButton.onClick.AddListener(OpenControl);
        audioButton.onClick.AddListener(OpenAudio);
        pauseButton.onClick.AddListener(OpenPause);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Load");
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    private void OpenSettings()
    {
        settingPanel.SetActive(true);
        OpenPause();
        //audioPanel.SetActive(false);
       // controlPanel.SetActive(false);
    }

    private void CloseSettings()
    {
        settingPanel.SetActive(false);
    }

    private void OpenControl()
    {
        ShowPanel(controlPanel);
        SetButtonActive(controlButton);

       // audioPanel.SetActive(false);
       // pausePanel.SetActive(false);
       // controlPanel.SetActive(true);
    }

    private void OpenAudio()
    {
        ShowPanel(audioPanel);
        SetButtonActive(audioButton);

        // controlPanel.SetActive(false);
        //pausePanel.SetActive(false);
        //audioPanel.SetActive(true);
    }

    private void OpenPause()
    {
        ShowPanel(pausePanel);
        SetButtonActive(pauseButton);
    }

    private void ShowPanel(GameObject panel)
    {
        // Hide all panels
        controlPanel.SetActive(false);
        audioPanel.SetActive(false);
        pausePanel.SetActive(false);

        // Show the selected panel
        panel.SetActive(true);
    }

    private void SetButtonActive(Button button)
    {
        // Reset previous button color (if any)
        if (activeButton != null)
        {
            activeButton.interactable = true;
        }

        // Disable interactability to show "pressed" effect
        button.interactable = false;
        activeButton = button;
    }


    /*  private void Setting()
      {
          pausePanel.SetActive(true);
          audioPanel.SetActive(false);
          controlPanel.SetActive(false);
      }
    */
    /*public GameObject settingPanel; // reference to the journal Ui panel
    public Button settingButton; // button to open setting
    public Button  closeButton; // button to close setting
    public GameObject controlPanel; // control panel
    public Button controlButton; // button for control
    public GameObject audioPanel; //  audio panel
    public Button audioButton; // button for audio
    public GameObject PausePanel;
    public Button PauseButton;


    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Load");
    }
    
    public void Setting()
    {
        settingPanel.SetActive(true);
    }
    public void Awake()
    {
        int screenW = 1920;
        int screenH = 1080;
        bool isFullScreen = true;

        Screen.SetResolution(screenW, screenH, isFullScreen);
    } */

}
