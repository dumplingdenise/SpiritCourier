using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public GameObject SettingPanel; // reference to the journal Ui panel
    public Button SettingButton; // button to open setting
    public Button CloseButton; // button to close setting
    public Animator playerAnimator; //player animation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Setting panel is not open
        SettingPanel.SetActive(false);

        SettingButton.onClick.AddListener(OpenSetting);
        CloseButton.onClick.AddListener(CloseSetting);
    }

    void OpenSetting()
    {
        SettingPanel.SetActive(true);
        Time.timeScale = 0f; // Pause Game

        if (playerAnimator != null)
        {
            playerAnimator.enabled = false;
        }
    }

    void CloseSetting()
    {
        SettingPanel.SetActive(false);
        Time.timeScale = 1f; //resume game

        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
