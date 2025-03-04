using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class OpeningScene : MonoBehaviour
{
    public GameObject Panel;  
    public GameObject TutorialPanel;
    public Text TutorialText;         
    public Button NextButton;              
    public Button StartGameButton;
    public Button SkipButton;
    public Image GlowingBall;

    public string[] tutorialMessages; //  Editable in Inspector!
    private int currentMessageIndex = 0;

    void Start()
    {
        Panel.SetActive(true);  // Hide tutorial at start
        TutorialPanel.SetActive(false);
        GlowingBall.gameObject.SetActive(true);

        ShowTutorial();
    }

    public void ShowTutorial()
    {
        Panel.SetActive(true); // Show tutorial
        currentMessageIndex = 0;
        TutorialText.text = tutorialMessages[currentMessageIndex];
        NextButton.gameObject.SetActive(true); // Show next button
        StartGameButton.gameObject.SetActive(false); // Hide start button
        SkipButton.gameObject.SetActive(true); // Show skip button
        GlowingBall.gameObject.SetActive(true);
    }

    public void NextTutorialMessage() // Toggle to the next message
    {
        currentMessageIndex++;

        if (currentMessageIndex < tutorialMessages.Length)
        {
            TutorialText.text = tutorialMessages[currentMessageIndex];
        }
        else
        {
            ShowTutorialPanel();
        }
    }

    public void ShowTutorialPanel()
    {
        Panel.SetActive(false);  // Hide text panel
        TutorialPanel.SetActive(true);      // Show tutorial panel
        NextButton.gameObject.SetActive(false);  // Hide next button
        StartGameButton.gameObject.SetActive(true); // Show start game button
        GlowingBall.gameObject.SetActive(true);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Movement");
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Denise1"); // Change to game scene
    }
}
