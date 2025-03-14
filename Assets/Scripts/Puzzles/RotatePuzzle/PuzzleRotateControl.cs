using UnityEngine;
using UnityEngine.UI;

public class PuzzleRotateControl : MonoBehaviour
{
    [SerializeField] private Transform[] pictures; // Puzzle pieces
    [SerializeField] private GameObject clueImageUI; // The UI Image for the clue
    [SerializeField] private GameObject OverlayUI; // The UI Image for the clue
    //[SerializeField] private Sprite clueSprite; // The full, unsliced clue image
    [SerializeField] private GameObject CrossUI;



    public static bool youWin;

    void Start()
    {
        GameController gameController = FindAnyObjectByType<GameController>(); // Find the GameController
        if (gameController != null)
        {
            gameController.SetGameState(GameState.Puzzle); // Set the state to FreeRoam when exiting the puzzle
        }
        youWin = false;
        clueImageUI.SetActive(false); // Hide the clue UI at start
        OverlayUI.SetActive(false); //hide overlay
        CrossUI.SetActive(false); //hide cross ui

    }

    void Update()
    {
        if (!youWin && IsPuzzleCompleted()) // Only trigger once
        {
            youWin = true;
            ShowClue();
            
        }
    }


    private bool IsPuzzleCompleted()
    {
        foreach (Transform piece in pictures)
        {
            if (piece.rotation.z != 0) // If any piece is not correctly rotated
                return false;
        }
        return true;
    }

    private void ShowClue()
    {
        clueImageUI.SetActive(true);
        OverlayUI.SetActive(true);
        CrossUI.SetActive(true);


        // Switch back to FreeRoam
        //clueImageUI.GetComponent<Image>().sprite = clueSprite;
        //OverlayUI.GetComponent<Image>().sprite = clueSprite;
        //Invoke("HideClue", 3f); // Hide clue after 5 seconds
    }
}

    
    
    
    /* private void HideClue()
    {
        
        clueImageUI.SetActive(false);
        OverlayUI.SetActive(false);
    }
    

} */
