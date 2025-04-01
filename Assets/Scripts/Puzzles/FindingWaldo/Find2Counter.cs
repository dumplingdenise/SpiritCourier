using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Find2Counter : MonoBehaviour
{
    public int totalParcels = 3; // Player needs to find 3 parcels
    private int foundParcels = 0; // Count of found parcels
    private bool npcFound = false; // Track if NPC is found

    public Text npcText; // UI text for NPC (e.g. 0/1)
    public Text parcelText; // UI text for parcels
    public GameObject puzzleCompletePanel; // UI pop-up when puzzle is done

    public GameObject npcIndicator; // Circle outline for NPC
    public GameObject[] parcelIndicators; // Circle outlines for parcels
    public Button[] parcelButtons; // Buttons for each parcel

    public GameObject CrossUI;
    public Button CloseButton; // button to close settings

    public Animator playerAnimator; // Player animation

    private void OnEnable()
    {
        ResetPuzzle(); // Reset puzzle each time it is activated
    }

    void Start()
    {
        CrossUI.SetActive(false);
        Time.timeScale = 0f; // Pause Game

        GameController gameController = FindAnyObjectByType<GameController>(); // Find the GameController
        if (gameController != null)
        {
            gameController.SetGameState(GameState.Puzzle);
            Debug.Log($"Game state when entering puzzle: {gameController.GetCurrentState()}");
        }

        // Assign listeners
        foreach (Button button in parcelButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        UpdateProgress();
    }

    public void FindNPC()
    {
        if (!npcFound)
        {
            npcFound = true;
            npcIndicator.SetActive(true);
            UpdateProgress();
        }
    }

    public void FindParcel(int parcelIndex)
    {
        if (foundParcels < totalParcels)
        {
            parcelIndicators[parcelIndex].SetActive(true);
            foundParcels++;
            UpdateProgress();
        }
    }

    void UpdateProgress()
    {
        npcText.text = npcFound ? "1/1" : "0/1";
        parcelText.text = $"{foundParcels}/{totalParcels}";

        if (foundParcels >= totalParcels && npcFound)
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

    public void ResetPuzzle()
    {
        Debug.Log("ResetPuzzle called");
        puzzleCompletePanel.SetActive(false);
        // Reset state variables
        foundParcels = 0;
        npcFound = false;

        // Hide all indicators
        npcIndicator.SetActive(false);
        foreach (GameObject indicator in parcelIndicators)
        {
            indicator.SetActive(false);
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

        UpdateProgress(); // Refresh UI
    }

    public void ClosePuzzle()
    {
        CrossUI.SetActive(false);
        Time.timeScale = 1f;
        ResetPuzzle(); // Ensure reset when closing
        this.gameObject.SetActive(false);
    }
}


/*using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Find2Counter : MonoBehaviour
{
    public int totalParcels = 3; // Player needs to find 3 parcels
    private int foundParcels = 0; // Count of found parcels
    private bool npcFound = false; // Track if NPC is found

    public Text npcText; // UI text for NPC (e.g. 0/1)
    public Text parcelText; // UI text for parcels
    public GameObject puzzleCompletePanel; // UI pop-up when puzzle is done

    public GameObject npcIndicator; // Circle outline for NPC
    public GameObject[] parcelIndicators; // Circle outlines for parcels
    public Button[] parcelButtons; // Buttons for each parcel, these should correspond to the ones on the image

    public GameObject CrossUI;
    public Button CloseButton; // button to close setting

    public Animator playerAnimator; //player animation

    // Start is called before the first frame update
    void Start()
    {
        CrossUI.SetActive(false);
        Time.timeScale = 0f; // Pause Game

        GameController gameController = FindAnyObjectByType<GameController>(); // Find the GameController
        if (gameController != null)
        {
            gameController.SetGameState(GameState.Puzzle); // Set the state to FreeRoam when exiting the puzzle
            Debug.LogError($"Game state when entering puzzle: {gameController.GetCurrentState()}");
        }
        UpdateProgress();

        // Assign each button's click listener to the FindParcel method
        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i; // Local copy for the loop
            parcelButtons[i].onClick.AddListener(() => FindParcel(index)); // Pass the correct index when the button is clicked
        }

        OpenPuzzle(); // Ensure the puzzle resets on start
    }

    // Called when entering the puzzle
    public void OpenPuzzle()
    {
        ResetPuzzleState(); // Ensure a fresh state when opening
        UpdateProgress();
    }

    // Reset the puzzle state
    void ResetPuzzleState()
    {
        npcFound = false;
        foundParcels = 0;
        puzzleCompletePanel.SetActive(false);

        npcIndicator.SetActive(false);

        // Disable all parcel indicators
        foreach (GameObject indicator in parcelIndicators)
        {
            indicator.SetActive(false);
        }

        // Remove listeners to prevent duplicates
        foreach (Button button in parcelButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        // Reassign listeners
        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i;
            parcelButtons[i].onClick.AddListener(() => FindParcel(index));
        }

        CrossUI.SetActive(false);

        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }

    // Called when NPC is clicked
    public void FindNPC()
    {
        if (!npcFound)
        {
            npcFound = true;
            npcIndicator.SetActive(true); // Show NPC indicator
            UpdateProgress();
        }
    }

    // Called when a parcel is clicked
    public void FindParcel(int parcelIndex)
    {
        if (foundParcels < totalParcels)
        {
            // Only show the indicator for the clicked parcel (based on the index)
            parcelIndicators[parcelIndex].SetActive(true);
            foundParcels++; // Increment the count of found parcels
            UpdateProgress();
        }
    }

    // Update progress
    void UpdateProgress()
    {
        npcText.text = npcFound ? "1/1" : "0/1"; // Update NPC text
        parcelText.text = $"{foundParcels}/{totalParcels}"; // Update parcel text

        if (foundParcels >= totalParcels && npcFound)
        {
            Debug.Log("Puzzle Completed!");
            PuzzleCompleted();
        }
    }

    void PuzzleCompleted()
    {
        puzzleCompletePanel.SetActive(true); // Show pop-up when puzzle is done
        CrossUI.SetActive(true);
        Time.timeScale = 1f; //resume game
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }
}
*/