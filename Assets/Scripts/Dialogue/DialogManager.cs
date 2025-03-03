using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{


    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    [SerializeField] Button nextSceneButton; // Add a UI button in the Inspector
    [SerializeField] string nextSceneName; // Scene name to load
    [SerializeField] Button noButton;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }

    Dialog currentDialog;
    int currentLine = 0;
    bool isTyping;
    bool showButtonAtEnd = false; // Flag to check if button should appear
    bool autoDialogCompleted = false; // Flag to check if auto-dialogu is complete

    public void Awake()
    {
        Instance = this;
        if (nextSceneButton != null)
        {
            nextSceneButton.gameObject.SetActive(false); // Hide button by default
            nextSceneButton.onClick.AddListener(GoToNextScene);

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

        showButtonAtEnd = showButton;
        yield return new WaitForEndOfFrame();

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
                    nextSceneButton.gameObject.SetActive(true); // Show button
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
    }

    public void GoToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
           SceneManager.LoadScene(nextSceneName);
        }
    }

    private void CloseDialog()
    {
        dialogBox.SetActive(false);
        currentLine = 0;
        autoDialogCompleted = false; // Reset auto-dialog flag
        noButton.gameObject.SetActive(false);
        nextSceneButton.gameObject.SetActive(false);
        OnHideDialog?.Invoke();
    }

    // New methods for success and failure dialog creation
    public Dialog CreateSuccessDialog(string parcelName)
    {
        return new Dialog
        {
            Lines = new List<string> { $"Thank you for delivering this parcel to me! It really means a lot to me!" }
        };
    }

    public Dialog CreateFailureDialog(/*string npcName*/)
    {
        return new Dialog
        {
            Lines = new List<string> { $"Oh no dear, I don't think this parcel is for me!" }
        };
    }

}
