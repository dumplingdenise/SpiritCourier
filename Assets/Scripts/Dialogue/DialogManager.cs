using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    [SerializeField] Button nextPuzzleSceneButton; // Add a UI button in the Inspector
/*    [SerializeField] string[] nextPuzzleSceneNames;*/
    [SerializeField] string[] easyPuzzleSceneNames;
    [SerializeField] string[] hardPuzzleSceneNames;

    [SerializeField] Button noButton;

    [SerializeField] private PlayerController playerController;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }

    private int npcInteractedCount = 0;

    Dialog currentDialog;
    int currentLine = 0;
    bool isTyping;

    private bool isDialogActive = false; // test

    bool showButtonAtEnd = false; // Flag to check if button should appear
    bool autoDialogCompleted = false; // Flag to check if auto-dialogue is complete

    public void Awake()
    {
        Instance = this;
        if (nextPuzzleSceneButton != null)
        {
            nextPuzzleSceneButton.gameObject.SetActive(false); // Hide button by default
            nextPuzzleSceneButton.onClick.AddListener(GoToNextScene);

            noButton.gameObject.SetActive(false);
            noButton.onClick.AddListener(CloseDialog);
        }
    }

    public IEnumerator ShowDialog(Dialog dialog, bool showButton = false)
    {
        if (dialog == null || dialog.Lines == null || dialog.Lines.Count == 0)
        {
            yield break;
        }

        if (isDialogActive)
        {
            yield break;
        }

        isDialogActive = true;

        showButtonAtEnd = showButton;
        yield return new WaitForEndOfFrame();

        // maybe dont need the below code at all
       /* if (playerController != null)
        {
            playerController.SetMoveWhenTalking(false);
        }*/

        OnShowDialog?.Invoke();

        currentDialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    private void Update()
    {
        HandleUpdate();
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyUp(KeyCode.F) && !isTyping)
        {
            if (currentDialog == null) return;

            ++currentLine;
            if (currentLine < currentDialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(currentDialog.Lines[currentLine]));
            }
            else
            {
                if (showButtonAtEnd)
                {
                    nextPuzzleSceneButton.gameObject.SetActive(true); // Show button
                    noButton.gameObject.SetActive(true);
                }
                else
                {
                    autoDialogCompleted = true;
                }
            }
        }

        // Close the dialog if auto-dialog has completed and player presses F
        if (autoDialogCompleted && Input.GetKeyUp(KeyCode.F))
        {
            CloseDialog();
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        if (string.IsNullOrEmpty(line)) yield break;

        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;

        // Now, wait for the player to press 'F' before continuing
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null; // Wait for the 'F' key press
        }

        // After 'F' is pressed, move to the next line
        if (currentLine + 1 < currentDialog.Lines.Count)
        {
            currentLine++;
            StartCoroutine(TypeDialog(currentDialog.Lines[currentLine]));
        }
        else
        {
            isDialogActive = false; // End of dialog
        }
    }

    public void GoToNextScene()
    {
        /*if (nextPuzzleSceneNames != null && nextPuzzleSceneNames.Length > 0)
        {
            string randomScene = nextPuzzleSceneNames[Random.Range(0, nextPuzzleSceneNames.Length)];
            if (!string.IsNullOrEmpty(randomScene))
            {
                SceneManager.LoadScene(randomScene);
            }
        }*/

        // test code - doesnt work cause the number of npcInteractedCount will always reset to 0 

        Debug.Log(npcInteractedCount);
        string randomSceneName = "";
        if ( npcInteractedCount < 2)
        {
            if (easyPuzzleSceneNames != null && easyPuzzleSceneNames.Length > 0)
            {
                randomSceneName = easyPuzzleSceneNames[Random.Range(0, easyPuzzleSceneNames.Length)];
            }
        }
        else
        {
            if (hardPuzzleSceneNames != null && hardPuzzleSceneNames.Length > 0)
            {
                randomSceneName = hardPuzzleSceneNames[Random.Range(0, hardPuzzleSceneNames.Length)];
            }
        }

        if (!string.IsNullOrEmpty(randomSceneName))
        {
            npcInteractedCount++;
            if (Quest.Instance != null)
            {
                Quest.Instance.HideQuestUI();
            }
            SceneManager.LoadScene(randomSceneName);
        }
    }

    private void CloseDialog()
    {
        dialogBox.SetActive(false);
        currentLine = 0;
        autoDialogCompleted = false; // Reset auto-dialog flag
        noButton.gameObject.SetActive(false);
        nextPuzzleSceneButton.gameObject.SetActive(false);

        OnHideDialog?.Invoke();

        // maybe dont need the below code at all
        /*if (playerController != null)
        {
            playerController.SetMoveWhenTalking(true);
        }*/ 
    }

    // New methods for success and failure dialog creation
    public Dialog CreateSuccessDialog()
    {
        return new Dialog
        {
            Lines = new List<string> { $"Thank you so much for the delivery!" }
        };
    }

    public Dialog CreateFailureDialog(/*string npcName*/)
    {
        return new Dialog
        {
            Lines = new List<string> { $"Oh no dear, I don't think this parcel is for me!" }
        };
    }

    // test
    public IEnumerator BackstoryRevealed(Dialog dialog)
    {
        // Make sure no other dialog is active before starting a new one
        if (isDialogActive)
        {
            yield return new WaitUntil(() => !isDialogActive); // Wait until the current dialog finishes
        }

        // Now that the previous dialog is finished, we can start the new one
        StartCoroutine(ShowDialog(dialog)); // This will handle showing the dialog
    }

    // test code
   /* public void DisplayQuestCount()
    {
        int questCount = Quest.Instance.GetQuest();
    }*/
}
