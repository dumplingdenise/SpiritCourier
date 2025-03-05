using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    public GameObject journalPanel; // Reference to the journal UI panel
    public Button openJournalButton; // Button to open the journal
    public Button closeJournalButton; // Button to close the journal
    public Animator playerAnimator; //player animation
    public Text JournalText;

    void Start()
    {
        // Hide journal at the start
        journalPanel.SetActive(false);

        // Add listeners to buttons
        openJournalButton.onClick.AddListener(OpenJournal);
        closeJournalButton.onClick.AddListener(CloseJournal);
    }

    void OpenJournal()
    {
        journalPanel.SetActive(true);
        /*JournalText.SetActive(false);*/
        Time.timeScale = 0f; // **PAUSE GAME**

        if (playerAnimator != null )
        {
            playerAnimator.enabled = false;
        }
    }

    void CloseJournal()
    {
        journalPanel.SetActive(false);
        Time.timeScale = 1f; // **RESUME GAME**

        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }
}
