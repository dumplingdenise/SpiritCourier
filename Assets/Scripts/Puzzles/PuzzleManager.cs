using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject puzzleGameObject; // reference to the entire puzzle gameobject
    [SerializeField] private GameObject puzzleUI;  // reference to the UI element that will show after the puzzle is completed
    [SerializeField] private GameController gameController; // reference to the GameController to manage the game state

    public static PuzzleManager startgame { get; private set; }

    

    private void Start()
    {
        puzzleGameObject.SetActive(false); // Hide puzzle at start
        puzzleUI.SetActive(false); // Hide the UI element
    }

    // Trigger the puzzle when the method is called
    public void StartPuzzle()
    {
        puzzleGameObject.SetActive(true); // Show the puzzle
        //state = GameState.Battle;  // Pause player movement during the puzzle
    }

    // Show UI and resume game when the puzzle is completed
    public void CompletePuzzle()
    {
        puzzleGameObject.SetActive(false); // Hide puzzle when completed
        puzzleUI.SetActive(true); // Show completion UI
        //state = GameState.FreeRoam;  // Resume player movement
    }
}
