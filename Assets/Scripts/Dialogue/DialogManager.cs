/*using System;
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

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        if (nextSceneButton != null)
        {
            nextSceneButton.gameObject.SetActive(false); // Hide button by default
            nextSceneButton.onClick.AddListener(GoToNextScene);
        }
    }

    Dialog currentDialog;
    int currentLine = 0;
    bool isTyping;
    bool showButtonAtEnd = false; // Flag to check if button should appear

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
                }
                else
                {
                    CloseDialog();
                }
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
        OnHideDialog?.Invoke();
    }

}
*/

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

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        if (nextSceneButton != null)
        {
            nextSceneButton.gameObject.SetActive(false); // Hide button by default
            nextSceneButton.onClick.AddListener(GoToNextScene);
        }
    }

    Dialog currentDialog;
    int currentLine = 0;
    bool isTyping;
    bool showButtonAtEnd = false; // Flag to check if button should appear

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
                }
                else
                {
                    CloseDialog();
                }
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
        OnHideDialog?.Invoke();
    }

}
