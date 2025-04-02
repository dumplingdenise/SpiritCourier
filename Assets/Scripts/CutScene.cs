using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName; // Change this to your tutorial scene's name

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Inventory.Instance != null)
        {
            Destroy(Inventory.Instance.gameObject);
            Debug.Log("Inventory destroyed in Cutscene scene.");
        }

        if (Quest.Instance != null)
        {
            Destroy(Quest.Instance.gameObject);
            Debug.Log("Quest destroyed in Cutscene scene.");
        }
        videoPlayer.loopPointReached += EndCutscene; // Detect when video ends
    }

    void EndCutscene(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Load next scene
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
