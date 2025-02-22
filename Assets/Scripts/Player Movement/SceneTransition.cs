using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public void GoToPuzzleScene(string sceneName)
    {
        if (PlayerPositionControl.Instance != null)
        {
            PlayerPositionControl.Instance.SavePosition(transform.position);
        }
        SceneManager.LoadScene(sceneName);
    }
}
