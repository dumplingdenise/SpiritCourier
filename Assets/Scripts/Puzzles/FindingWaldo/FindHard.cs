using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class FindHard : MonoBehaviour
{
    public int totalParcels = 3; // Number of parcels needed
    private int foundParcels = 0; // Count of found parcels

    public int totalNPCs = 2; // Number of NPCs to find (change as needed)
    private int foundNPCs = 0; // Track how many NPCs were found

    public Text npcText; // UI text for NPC count
    public Text parcelText; // UI text for parcel count
    public GameObject puzzleCompletePanel; // UI pop-up when puzzle is done

    public GameObject[] npcIndicators; // Array for NPC indicators
    public Button[] npcButtons; // Buttons for each NPC (if clickable)
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
        for (int i = 0; i < npcButtons.Length; i++)
        {
            int index = i;
            npcButtons[i].onClick.AddListener(() => FindNPC(index));
        }
    }

    void ResetPuzzle()
    {
        // Reset values
        foundParcels = 0;
        foundNPCs = 0;

        // Hide indicators
        foreach (GameObject npc in npcIndicators)
            npc.SetActive(false);

        foreach (GameObject parcel in parcelIndicators)
            parcel.SetActive(false);

        // Reset UI
        puzzleCompletePanel.SetActive(false);
        CrossUI.SetActive(false);
        UpdateProgress();
    }

    // Called when an NPC is clicked
    public void FindNPC(int npcIndex)
    {
        if (foundNPCs < totalNPCs && !npcIndicators[npcIndex].activeSelf)
        {
            npcIndicators[npcIndex].SetActive(true); // Show the NPC indicator
            foundNPCs++;
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
        npcText.text = $"{foundNPCs}/{totalNPCs}";
        parcelText.text = $"{foundParcels}/{totalParcels}";

        if (foundParcels >= totalParcels && foundNPCs >= totalNPCs)
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
