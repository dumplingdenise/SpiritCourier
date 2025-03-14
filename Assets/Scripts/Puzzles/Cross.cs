using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Cross : MonoBehaviour
{
    public void ExitButton()
    {
        // Find and disable the AudioListener in the current scene
        /*        AudioListener audioListener = Object.FindFirstObjectByType<AudioListener>();
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

        if (Quest.Instance != null)
        {
            Quest.Instance.ShowQuestUI();
        }
        SceneManager.LoadScene("Denise");

        // test
        /* SceneManager.LoadScene("Trial");*/
    }
}
