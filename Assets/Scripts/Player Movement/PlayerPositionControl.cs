using UnityEngine;

public class PlayerPositionControl : MonoBehaviour
{
    public static PlayerPositionControl Instance;
    private Vector3 savedPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load saved position if it exists
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            savedPosition = new Vector3(x, y, z);
            transform.position = savedPosition;

            PauseGame();
        }
        gameObject.SetActive(true);
    }

    public void SavePosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.SetFloat("PlayerZ", position.z);
        PlayerPrefs.Save();

        UnpauseGame();
    }

    // Call this function when the player is about to enter the puzzle scene
    public void PauseGame()
    {
        Time.timeScale = 0f;  // Pause the game
    }

    // Call this function when the player returns from the puzzle scene
    public void UnpauseGame()
    {
        Time.timeScale = 1f;  // Resume the game
    }
}
