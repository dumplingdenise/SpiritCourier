using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cross : MonoBehaviour
{
    public GameObject puzzlePanel; // Assign in Inspector
 /*   public void ExitButton()
    {
        // Find and disable the AudioListener in the current scene
               AudioListener audioListener = Object.FindFirstObjectByType<AudioListener>();
                if (audioListener != null)
                {
                    audioListener.enabled = false; // Disable the AudioListener from the current scene
                }*/

        /*SceneManager.LoadScene("Play");*/

        // test code does not work
        /*// Get the GameController in the current scene
        GameController gameController = *//*FindObjectOfType<GameController>();*//* FindAnyObjectByType<GameController>();
        if (gameController != null)
        {
            gameController.SetGameState(GameState.FreeRoam);
        }*/

        /*  if (Quest.Instance != null)
          {
              Quest.Instance.ShowQuestUI();
          }
          SceneManager.LoadScene("Shumin");
          Time.timeScale = 1f; //resume game
        
    }
        */
    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(ClosePuzzle);
        }
    }

    public void ClosePuzzle()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false); // Hide the puzzle UI
            Time.timeScale = 1f; // Resume the game
        }

        GameController gameController = FindAnyObjectByType<GameController>();
        DialogManager dialogMang = FindAnyObjectByType<DialogManager>();
        if (gameController != null)
        {
            // test 
            /*if (inventoryManager.GetInventoryList() != null)
            {
                gameController.SetGameState(GameState.WaitingForDelivery); // Resume gameplay
            }
            gameController.SetGameState(GameState.FreeRoam);*/


            gameController.SetGameState(GameState.WaitingForDelivery); // Resume gameplay
            dialogMang.CloseDialog();

        }


}
}
