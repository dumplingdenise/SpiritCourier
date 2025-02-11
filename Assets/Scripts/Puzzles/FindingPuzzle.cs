using UnityEngine;
using UnityEngine.UI;

public class FindingPuzzle : MonoBehaviour
{
 
    [SerializeField] private GameObject waldo; // The Waldo object to find
    [SerializeField] private GameObject puzzleUI; // The UI panel containing the puzzle
    [SerializeField] private Text feedbackText; // Feedback message UI

    private bool waldoFound = false;

    void Start()
    {
        puzzleUI.SetActive(false); // Initially hide the puzzle
    }

    // Call this method when the puzzle is triggered
    public void StartPuzzle()
    {
        puzzleUI.SetActive(true); // Show the puzzle UI
        feedbackText.text = "Find the Blue Spirit!"; // Give the player an instruction
        waldoFound = false; // Reset the puzzle

        // Automatically hide the puzzle after a few seconds
        Invoke("ClosePuzzle", 10f); // You can change the time if needed
    }

    // Check if the player clicked near Waldo's position
    public void CheckForWaldo(Vector3 clickPosition)
    {
        if (Vector3.Distance(clickPosition, waldo.transform.position) < 50f) // Allow for some margin of error
        {
            WaldoFound();
        }
        else
        {
            feedbackText.text = "Not quite! Keep looking!";
        }
    }

    // Handle when Waldo is found
    private void WaldoFound()
    {
        if (!waldoFound)
        {
            waldoFound = true;
            feedbackText.text = "You found Waldo! Congratulations!";
        }
    }

    // Close the puzzle after the specified time
    private void ClosePuzzle()
    {
        puzzleUI.SetActive(false); // Hide the puzzle UI
    }
}


