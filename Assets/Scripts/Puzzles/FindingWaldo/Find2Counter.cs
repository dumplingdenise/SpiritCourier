using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        UpdateProgress();

        // Assign each button's click listener to the FindParcel method
        for (int i = 0; i < parcelButtons.Length; i++)
        {
            int index = i; // Local copy for the loop
            parcelButtons[i].onClick.AddListener(() => FindParcel(index)); // Pass the correct index when the button is clicked
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
    }
}
