using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    public GameObject Panel;
    public Text TutorialText;
    public Button NextButton;
    public Button SkipButton;

    public string[] tutorialMessages; //  Editable in Inspector!
    private int currentMessageIndex = 0;

    void Start()
    {
        Panel.SetActive(true);  // Hide tutorial at start

        ShowTutorial();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Toggle visibility when F is pressed
        {
            Panel.SetActive(!Panel.activeSelf);
        }
    }

    public void ShowTutorial()
    {
        Panel.SetActive(true); // Show tutorial
        currentMessageIndex = 0;
        TutorialText.text = tutorialMessages[currentMessageIndex];
        NextButton.gameObject.SetActive(true); // Show next button
        SkipButton.gameObject.SetActive(true); // Show skip button
    }

    public void HideTutorial()
    {
        Panel.SetActive(false); // Hide the entire tutorial UI
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
            LoadMainGame();
        }
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Denise"); // Change to game scene
    }
}
