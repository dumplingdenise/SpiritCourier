using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Cross : MonoBehaviour
{
    public void ExitButton()
    {
        // Find and disable the AudioListener in the current scene
        AudioListener audioListener = Object.FindFirstObjectByType<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = false; // Disable the AudioListener from the current scene
        }

        /*SceneManager.LoadScene("Play");*/

        SceneManager.LoadScene("Denise");
        
    }
}
