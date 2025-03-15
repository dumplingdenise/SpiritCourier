using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    public GameObject SettingPanel; // reference to the journal Ui panel
    public Button SettingButton; // button to open setting
    public Button CloseButton; // button to close setting
    public Animator playerAnimator; //player animation

    public GameObject ControlPanel; // control panel
    public Button ControlButton; // button for control
    public GameObject AudioPanel; //  audio panel
    public Button AudioButton; // button for audio
    public GameObject PausePanel;
    public Button PauseButton;

    private Button activeButton; // keep track of the currently 'pressed' button

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Setting panel is not open
        SettingPanel.SetActive(false);

        SettingButton.onClick.AddListener(OpenSetting);
        CloseButton.onClick.AddListener(CloseSetting);

        if (ControlButton != null)
        {
            AudioPanel.SetActive(false);
            ControlPanel.SetActive(false);

            ControlButton.onClick.AddListener(OpenControl);
        }
        else
        {
            AudioPanel.SetActive(false);
            ControlPanel.SetActive(false);
        }

        if (AudioPanel != null)
        {
            ControlPanel.SetActive(false);
            AudioPanel.SetActive(false);

            AudioButton.onClick.AddListener(OpenAudio);
        }
        else
        {
            AudioPanel.SetActive(false);
            ControlPanel.SetActive(false);
        }

        if(PausePanel != null)
        {
            PausePanel.SetActive(false);

            PauseButton.onClick.AddListener(OpenPause);
        }
    }

    void OpenSetting()
    {
        SettingPanel.SetActive(true);
        PausePanel.SetActive(true);
        AudioPanel.SetActive(false);
        SetButtonActive(PauseButton);
        Time.timeScale = 0f; // Pause Game

        if (playerAnimator != null)
        {
            playerAnimator.enabled = false;
        }
    }

    void CloseSetting()
    {
        SettingPanel.SetActive(false);
        Time.timeScale = 1f; //resume game

        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }

    void OpenControl()
    {
        SetButtonActive(ControlButton);
        AudioPanel.SetActive(false);
        PausePanel.SetActive(false);
        ControlPanel.SetActive(true);
    }

    void OpenAudio()
    {
        SetButtonActive(AudioButton);
        ControlPanel.SetActive(false);
        PausePanel.SetActive(false);
        AudioPanel.SetActive(true);
    }

    void OpenPause()
    {
        SetButtonActive(PauseButton);
        ControlPanel.SetActive(false);
        PausePanel.SetActive(true);
        AudioPanel.SetActive(false);
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

    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

}
