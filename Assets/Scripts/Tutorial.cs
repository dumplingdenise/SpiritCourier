using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public GameObject Panel;
    public GameObject DialogBox;
    public GameObject JournalPanel;
    public GameObject BigMap;
    public Text TutorialText;
    public Button NextButton;
    public Button SkipButton;
   

    public string[] tutorialMessages; //  Editable in Inspector!
    private int currentMessageIndex = 0;
    private bool isPanelVisible = true; // Track the visibility state of the panel    

    void Start()
    {
        Panel.SetActive(true);

        ShowTutorial();
    }

    void Update()
    {
        // Check if DialogueManager is showing a dialog
        if (DialogBox != null && DialogBox.activeSelf)
        {
            Panel.SetActive(false); // Hide tutorial panel when dialogue is active
        }
        else if (JournalPanel != null && JournalPanel.activeSelf)
        {
            Panel.SetActive(false);
        }       
        else if (BigMap != null && BigMap.activeSelf)
        {
            Panel.SetActive(false);
        }
        else
        {
            Panel.SetActive(true);
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
        Debug.Log("Next button clicked.");
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
