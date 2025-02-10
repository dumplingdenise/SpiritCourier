using UnityEngine;
using UnityEngine.UI;

public class PuzzleRotateControl : MonoBehaviour
{
    [SerializeField] private Transform[] pictures; // Puzzle pieces
    [SerializeField] private GameObject clueImageUI; // The UI Image for the clue
    [SerializeField] private Sprite clueSprite; // The full, unsliced clue image

    public static bool youWin;

    void Start()
    {
        youWin = false;
        clueImageUI.SetActive(false); // Hide the clue UI at start
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
        clueImageUI.GetComponent<Image>().sprite = clueSprite;
        Invoke("HideClue", 3f); // Hide clue after 5 seconds
    }

    private void HideClue()
    {
        
        clueImageUI.SetActive(false);
    }

}
