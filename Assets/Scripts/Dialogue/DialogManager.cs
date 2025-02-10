using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    Dialog currentDialog;
    int currentLine = 0;
    bool isTyping;
   
    public IEnumerator ShowDialog(Dialog dialog)
    {
        if (dialog == null || dialog.Lines == null || dialog.Lines.Count == 0)
        {
            yield break; // Prevent errors if dialog is null or empty
        }

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
            ++currentLine;
            if (currentLine < currentDialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(currentDialog.Lines[currentLine]));
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;
                OnHideDialog?.Invoke();
            }                       
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
}
