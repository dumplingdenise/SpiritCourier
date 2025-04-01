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
