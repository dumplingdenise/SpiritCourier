using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Find5 : MonoBehaviour
{
    public int totalParcels = 3;
    private int foundParcels = 0;

    public int totalBlueKids = 3;
    private int foundBlueKids = 0;

    public int totalBlueGrannys = 2;
    private int foundBlueGrannys = 0;

    private bool PlayerFound = false;

    public Text BlueKidText;
    public Text BlueGrannyText;
    public Text parcelText;
    public Text PlayerText;
    public GameObject puzzleCompletePanel;

    public GameObject[] BlueKidIndicators;
    public Button[] BlueKidButtons;
    public GameObject[] BlueGrannyIndicators;
    public Button[] BlueGrannyButtons;
    public GameObject[] parcelIndicators;
    public Button[] parcelButtons;
    public GameObject PlayerIndicator; // Circle outline for NPC

    public GameObject CrossUI;
    public Button CloseButton;

    public Animator playerAnimator;

    // Hint System Variables
    public Button hintButton;
    public GameObject[] BlueKidHintIndicators; // Hint visual effects
    private int hintCount = 0;
    private int lastHintIndex = -1; // Keep track of last hinted BlueKid

    private void OnEnable()
    {
        ResetPuzzle(); // Reset puzzle each time it is activated
    }

    void Start()
    {
        ResetPuzzle();
        CrossUI.SetActive(false);
        Time.timeScale = 0f;

        GameController gameController = FindAnyObjectByType<GameController>();
        if (gameController != null)
        {
            gameController.SetGameState(GameState.Puzzle);
            Debug.Log($"Game state when entering puzzle: {gameController.GetCurrentState()}");
        }
        UpdateProgress();

        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        for (int i = 0; i < BlueKidButtons.Length; i++)
        {
            int index = i;
            BlueKidButtons[i].onClick.AddListener(() => FindKid(index));
        }

        for (int i = 0; i < BlueGrannyButtons.Length; i++)
        {
            int index = i;
            BlueGrannyButtons[i].onClick.AddListener(() => FindGranny(index));
        }

        // Connect hint button
        hintButton.onClick.AddListener(ShowHint);
        foreach (GameObject hint in BlueKidHintIndicators)
        {
            hint.SetActive(false); // Hide all hints initially
        }
    }

   /* void ResetPuzzle()
    {
        puzzleCompletePanel.SetActive(false);
        CrossUI.SetActive(false);

        foundParcels = 0;
        foundBlueKids = 0;
        foundBlueGrannys = 0;
        PlayerFound = false;
        PlayerIndicator.SetActive(false);
        hintCount = 0;
        lastHintIndex = -1; // Reset hint tracking

        foreach (GameObject npc in BlueKidIndicators)
        {
            npc.SetActive(false);
        }

        foreach (GameObject npc in BlueGrannyIndicators)
        {
            npc.SetActive(false);
        }

        foreach (GameObject parcel in parcelIndicators)
        {
            parcel.SetActive(false);
        }

        foreach (GameObject hint in BlueKidHintIndicators)
        {
            hint.SetActive(false); // Hide all hint indicators
        }

        // Unassign and reassign listeners
        foreach (Button button in parcelButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        // Unassign and reassign listeners
        foreach (Button button in BlueKidButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < BlueKidButtons.Length; i++)
        {
            int index = i;
            BlueKidButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        // Unassign and reassign listeners
        foreach (Button button in BlueGrannyButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < BlueGrannyButtons.Length; i++)
        {
            int index = i;
            BlueGrannyButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        UpdateProgress();

        hintButton.interactable = true; // Reactivate hint button
    }*/

    public void FindKid(int KidIndex)
    {
        if (foundBlueKids < totalBlueKids && !BlueKidIndicators[KidIndex].activeSelf)
        {
            BlueKidIndicators[KidIndex].SetActive(true);
            BlueKidHintIndicators[KidIndex].SetActive(false); // Hide hint if found
            foundBlueKids++;
            UpdateProgress();
        }
    }

    public void FindGranny(int GrannyIndex)
    {
        if (foundBlueGrannys < totalBlueGrannys && !BlueGrannyIndicators[GrannyIndex].activeSelf)
        {
            BlueGrannyIndicators[GrannyIndex].SetActive(true);
            foundBlueGrannys++;
            UpdateProgress();
        }
    }

    public void FindParcel(int parcelIndex)
    {
        if (foundParcels < totalParcels && !parcelIndicators[parcelIndex].activeSelf)
        {
            parcelIndicators[parcelIndex].SetActive(true);
            foundParcels++;
            UpdateProgress();
        }
    }

    public void FindPlayer()
    {
        if (!PlayerFound)
        {
            PlayerFound = true;
            PlayerIndicator.SetActive(true);
            UpdateProgress();
        }
    }

    void UpdateProgress()
    {
        BlueKidText.text = $"{foundBlueKids}/{totalBlueKids}";
        BlueGrannyText.text = $"{foundBlueGrannys}/{totalBlueGrannys}";
        parcelText.text = $"{foundParcels}/{totalParcels}";
        PlayerText.text = PlayerFound ? "1/1" : "0/1";

        if (foundParcels >= totalParcels && foundBlueKids >= totalBlueKids && foundBlueGrannys >= totalBlueGrannys && PlayerFound)
        {
            Debug.Log("Puzzle Completed!");
            PuzzleCompleted();
        }
    }

    void PuzzleCompleted()
    {
        puzzleCompletePanel.SetActive(true);
        CrossUI.SetActive(true);
        Time.timeScale = 1f;
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }

    // HINT FUNCTION: Allows multiple hints until all BlueKids are found
    public void ShowHint()
    {
        // Disable if all BlueKids are already found
        if (foundBlueKids >= totalBlueKids)
        {
            hintButton.interactable = false;
            return;
        }

        int nextHintIndex = -1;

        // Find the next unfound BlueKid that was not hinted before
        for (int i = 0; i < totalBlueKids; i++)
        {
            if (!BlueKidIndicators[i].activeSelf && i != lastHintIndex)
            {
                nextHintIndex = i;
                break;
            }
        }

        if (nextHintIndex != -1)
        {
            lastHintIndex = nextHintIndex;

            // Hide all hints first
            foreach (GameObject hint in BlueKidHintIndicators)
            {
                hint.SetActive(false);
            }

            // Show only one hint
            BlueKidHintIndicators[nextHintIndex].SetActive(true);

            // Start Coroutine to hide after delay
            StartCoroutine(HideHintAfterDelay(2f, nextHintIndex));
        }
    }

    // CORRECTED ResetPuzzle METHOD
    void ResetPuzzle()
    {
        Debug.Log("ResetPuzzle called");

        puzzleCompletePanel.SetActive(false);
        CrossUI.SetActive(false);

        foundParcels = 0;
        foundBlueKids = 0;
        foundBlueGrannys = 0;
        PlayerFound = false;
        PlayerIndicator.SetActive(false);

        // Reset hint tracking
        hintCount = 0;
        lastHintIndex = -1;
        hintButton.interactable = true;

        // Hide indicators
        foreach (GameObject npc in BlueKidIndicators) npc.SetActive(false);
        foreach (GameObject npc in BlueGrannyIndicators) npc.SetActive(false);
        foreach (GameObject parcel in parcelIndicators) parcel.SetActive(false);
        foreach (GameObject hint in BlueKidHintIndicators) hint.SetActive(false);

        // Clear existing listeners
        foreach (Button button in parcelButtons) button.onClick.RemoveAllListeners();
        foreach (Button button in BlueKidButtons) button.onClick.RemoveAllListeners();
        foreach (Button button in BlueGrannyButtons) button.onClick.RemoveAllListeners();

        // Re-add listeners
        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        for (int i = 0; i < BlueKidButtons.Length; i++)
        {
            int index = i;
            BlueKidButtons[i].onClick.AddListener(() => FindKid(index));
        }

        for (int i = 0; i < BlueGrannyButtons.Length; i++)
        {
            int index = i;
            BlueGrannyButtons[i].onClick.AddListener(() => FindGranny(index));
        }

        UpdateProgress();
    }

    /*  // HINT FUNCTION: Show one hint at a time, up to 2 hints total
      public void ShowHint()
      {
          if (hintCount >= 2)
          {
              hintButton.interactable = false; // Disable hint button after 2 uses
              return;
          }

          int nextHintIndex = -1;

          // Find the next unfound BlueKid that was not hinted before
          for (int i = 0; i < totalBlueKids; i++)
          {
              if (!BlueKidIndicators[i].activeSelf && i != lastHintIndex)
              {
                  nextHintIndex = i;
                  break;
              }
          }

          if (nextHintIndex != -1)
          {
              lastHintIndex = nextHintIndex; // Update last hinted BlueKid

              // Hide all hints first
              foreach (GameObject hint in BlueKidHintIndicators)
              {
                  hint.SetActive(false);
              }

              // Show only one hint
              BlueKidHintIndicators[nextHintIndex].SetActive(true);
              hintCount++;

              StartCoroutine(HideHintAfterDelay(2f, nextHintIndex)); // Hide after 3 sec
          }

          // Disable the hint button after 2 uses
          if (hintCount >= 2)
          {
              hintButton.interactable = false;
          }
      }*/

    IEnumerator HideHintAfterDelay(float delay, int hintIndex)
    {
        yield return new WaitForSecondsRealtime(delay);
        BlueKidHintIndicators[hintIndex].SetActive(false);
    }

    public void ClosePuzzle()
    {
        CrossUI.SetActive(false);
        Time.timeScale = 1f;
        ResetPuzzle(); // Ensure reset when closing
        this.gameObject.SetActive(false);
    }
}
