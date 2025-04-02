using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FindHard2 : MonoBehaviour
{
    public int totalParcels = 3;
    private int foundParcels = 0;

    public int totalBlueKids = 3;
    private int foundBlueKids = 0;

    public int totalBlueGrannys = 2;
    private int foundBlueGrannys = 0;

    public Text BlueKidText;
    public Text BlueGrannyText;
    public Text parcelText;
    public GameObject puzzleCompletePanel;

    public GameObject[] BlueKidIndicators;
    public Button[] BlueKidButtons;
    public GameObject[] BlueGrannyIndicators;
    public Button[] BlueGrannyButtons;
    public GameObject[] parcelIndicators;
    public Button[] parcelButtons;

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

    void ResetPuzzle()
    {
        puzzleCompletePanel.SetActive(false);
        CrossUI.SetActive(false);

        foundParcels = 0;
        foundBlueKids = 0;
        foundBlueGrannys = 0;
        hintCount = 0; // Reset hint count
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

        foreach (Button button in parcelButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        foreach (Button button in BlueKidButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < BlueKidButtons.Length; i++)
        {
            int index = i;
            BlueKidButtons[i].onClick.AddListener(() => FindKid(index));
        }

        foreach (Button button in BlueGrannyButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < BlueGrannyButtons.Length; i++)
        {
            int index = i;
            BlueGrannyButtons[i].onClick.AddListener(() => FindGranny(index));
        }

        UpdateProgress();

        hintButton.interactable = true; // Ensure button reactivates
    }


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

    void UpdateProgress()
    {
        BlueKidText.text = $"{foundBlueKids}/{totalBlueKids}";
        BlueGrannyText.text = $"{foundBlueGrannys}/{totalBlueGrannys}";
        parcelText.text = $"{foundParcels}/{totalParcels}";

        if (foundParcels >= totalParcels && foundBlueKids >= totalBlueKids && foundBlueGrannys >= totalBlueGrannys)
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

    public void ShowHint()
    {
        if (hintCount >= 2)
        {
            hintButton.interactable = false; // Disable button after two hints
            return;
        }

        int nextHintIndex = -1;

        // Loop to find the next hint
        for (int i = 0; i < totalBlueKids; i++)
        {
            if (!BlueKidIndicators[i].activeSelf && i != lastHintIndex)
            {
                nextHintIndex = i;
                break;
            }
        }

        // If no valid hint found, reset lastHintIndex to allow new hints
        if (nextHintIndex == -1)
        {
            lastHintIndex = -1;
            for (int i = 0; i < totalBlueKids; i++)
            {
                if (!BlueKidIndicators[i].activeSelf)
                {
                    nextHintIndex = i;
                    break;
                }
            }
        }

        // Show hint if a valid index is found
        if (nextHintIndex != -1)
        {
            lastHintIndex = nextHintIndex;

            foreach (GameObject hint in BlueKidHintIndicators)
            {
                hint.SetActive(false); // Hide all hints first
            }

            BlueKidHintIndicators[nextHintIndex].SetActive(true); // Show the selected hint
            hintCount++; // Increment hint count
            StartCoroutine(HideHintAfterDelay(2f, nextHintIndex)); // Hide after 2 seconds
        }

        // Disable the hint button if max hints used
        if (hintCount >= 2)
        {
            hintButton.interactable = false; // Disable hint button after two hints
        }
    }



    /*
        // HINT FUNCTION: Show one hint at a time, up to 2 hints total
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




/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FindHard2 : MonoBehaviour
{
    public int totalParcels = 3; // Number of parcels needed
    private int foundParcels = 0; // Count of found parcels

    public int totalBlueKids = 3; // Number of NPCs to find (change as needed)
    private int foundBlueKids = 0; // Track how many NPCs were found

    public int totalBlueGrannys = 2; // Number of NPCs to find (change as needed)
    private int foundBlueGrannys = 0; // Track how many NPCs were found

    public Text BlueKidText; // UI text for NPC count
    public Text BlueGrannyText; // UI text for NPC count
    public Text parcelText; // UI text for parcel count
    public GameObject puzzleCompletePanel; // UI pop-up when puzzle is done

    public GameObject[] BlueKidIndicators; // Array for NPC indicators
    public Button[] BlueKidButtons; // Buttons for each NPC (if clickable)
    public GameObject[] BlueGrannyIndicators; // Array for NPC indicators
    public Button[] BlueGrannyButtons; // Buttons for each NPC (if clickable)
    public GameObject[] parcelIndicators; // Array for parcel indicators
    public Button[] parcelButtons; // Buttons for each parcel

    public GameObject CrossUI;
    public Button CloseButton; // Button to close UI

    public Animator playerAnimator; // Player animation

    void Start()
    {
        ResetPuzzle(); // Reset everything at the start
        CrossUI.SetActive(false);
        Time.timeScale = 0f; // Pause Game

        GameController gameController = FindAnyObjectByType<GameController>();
        if (gameController != null)
        {
            gameController.SetGameState(GameState.Puzzle);
            Debug.Log($"Game state when entering puzzle: {gameController.GetCurrentState()}");
        }
        UpdateProgress();

        // Assign each parcel button a listener
        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        // Assign each NPC button a listener
        for (int i = 0; i < BlueKidButtons.Length; i++)
        {
            int index = i;
            BlueKidButtons[i].onClick.AddListener(() => FindKid(index));
        }

        // Assign each NPC button a listener
        for (int i = 0; i < BlueGrannyButtons.Length; i++)
        {
            int index = i;
            BlueGrannyButtons[i].onClick.AddListener(() => FindGranny(index));
        }

        // Connect hint button
        hintButton.onClick.AddListener(ShowHint);
        hintIndicator.SetActive(false); // Hide hint indicator
    }

    void ResetPuzzle()
    {
        // Reset values
        foundParcels = 0;
        foundBlueKids = 0;
        foundBlueGrannys = 0;

        // Hide indicators
        foreach (GameObject npc in BlueKidIndicators)
            npc.SetActive(false);
        
        // Hide indicators
        foreach (GameObject npc in BlueGrannyIndicators)
            npc.SetActive(false);

        foreach (GameObject parcel in parcelIndicators)
            parcel.SetActive(false);

        // Reset UI
        puzzleCompletePanel.SetActive(false);
        CrossUI.SetActive(false);
        UpdateProgress();
    }

    // Called when an NPC is clicked
    public void FindKid(int KidIndex)
    {
        if (foundBlueKids < totalBlueKids && !BlueKidIndicators[KidIndex].activeSelf)
        {
            BlueKidIndicators[KidIndex].SetActive(true); // Show the NPC indicator
            foundBlueKids++;
            UpdateProgress();
        }
    }

    public void FindGranny(int GrannyIndex)
    {
        if (foundBlueGrannys < totalBlueGrannys && !BlueGrannyIndicators[GrannyIndex].activeSelf)
        {
            BlueGrannyIndicators[GrannyIndex].SetActive(true); // Show the NPC indicator
            foundBlueGrannys++;
            UpdateProgress();
        }
    }

    // Called when a parcel is clicked
    public void FindParcel(int parcelIndex)
    {
        if (foundParcels < totalParcels && !parcelIndicators[parcelIndex].activeSelf)
        {
            parcelIndicators[parcelIndex].SetActive(true);
            foundParcels++;
            UpdateProgress();
        }
    }

    // Update progress
    void UpdateProgress()
    {
        BlueKidText.text = $"{foundBlueKids}/{totalBlueKids}";
        BlueGrannyText.text = $"{foundBlueGrannys}/{totalBlueGrannys}";
        parcelText.text = $"{foundParcels}/{totalParcels}";

        if (foundParcels >= totalParcels && foundBlueKids >= totalBlueKids && foundBlueGrannys >= totalBlueGrannys)
        {
            Debug.Log("Puzzle Completed!");
            PuzzleCompleted();
        }
    }

    void PuzzleCompleted()
    {
        puzzleCompletePanel.SetActive(true);
        CrossUI.SetActive(true);
        Time.timeScale = 1f; // Resume game
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }
}
*/