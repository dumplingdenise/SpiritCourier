using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    public static JournalUI Instance;
    public GameObject journalPanel;
    public Button openJournalButton;
    public Button closeJournalButton;
    public Animator playerAnimator;

    public GameObject[] journalPages; // Array of pages
    public Button nextPageButton;
    public Button prevPageButton;
    private int currentPage = 0;
    private int totalPagesUnlocked = 0;
    public GameObject notificationCircle;
    private List<GameObject> unlockedPages = new List<GameObject>();


    private Dictionary<int, GameObject> journalEntries = new Dictionary<int, GameObject>(); // Maps NPC ID to pages

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of JournalUI detected!");
        }

        journalPanel.SetActive(false);

        
        openJournalButton.onClick.AddListener(OpenJournal);
        closeJournalButton.onClick.AddListener(CloseJournal);

        notificationCircle.SetActive(false);

        InitializeJournalEntries();
    }

    void InitializeJournalEntries()
    {
        if (journalPages.Length == 0)  // If journal pages is empty, it logs an error and exits the function
        {
            Debug.LogError("No journal pages assigned!");
            return;
        }

        MainNpcs mainNpcs = FindFirstObjectByType<MainNpcs>();  // Finds main npc script, if it doesnt exist, then it will exit
        if (mainNpcs == null)
        {
            Debug.LogError("MainNpcs script not found!");
            return;
        }

        List<MainNpcs.NPCData> npcList = mainNpcs.GetNPCList();                     // Getting a the list of NPC stored in MainNPC

        if (npcList.Count > journalPages.Length)                                    // Ensures there are enough journal pages to match the number of NPCs
        {
            Debug.LogError("Not enough journal pages for the number of NPCs!");
            return;
        }

        for (int i = 0; i < npcList.Count; i++)                                     // Loops through all NPCs and assigns a journal page to each npcID
        {
            journalEntries[npcList[i].npcID] = journalPages[i];                     // Stores this mapping in journalEntries (dictionary)
            journalPages[i].SetActive(false);                                       // Hides all journal entries from the start
            Debug.Log($"Journal page {i} assigned to NPC ID {npcList[i].npcID}");   // Logs a debug message showing which page is assigned to which NPC
        }
    }

    public void OpenJournal()
    {
        journalPanel.SetActive(true);
        Time.timeScale = 0f;

        if (playerAnimator != null)
        {
            playerAnimator.enabled = false;
        }

        if (notificationCircle.activeSelf)
        {
            notificationCircle.SetActive(false);
        }
    }

    public void CloseJournal()
    {
        journalPanel.SetActive(false);
        Time.timeScale = 1f;

        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }
    }

    public void AddJournalEntry(int npcID)
    {
        Debug.Log($"Add Journal Entry called for id {npcID}");

        if (journalEntries.ContainsKey(npcID))
        {
            GameObject unlockedPage = journalEntries[npcID];
            //unlockedPage.SetActive(true);
            unlockedPages.Add(unlockedPage); // Add the unlocked page to the list
            currentPage = unlockedPages.Count - 1; // Set the current page to the last unlocked one
            UpdatePageVisibility();
            notificationCircle.SetActive(true);
            Debug.Log($"current number of pages {unlockedPages.Count}");
        }
        else
        {
            Debug.LogError($"No journal entry found for NPC ID {npcID}");
        }

        
    }


    public void NextPage()
    {
        Debug.Log($"Check the current page {currentPage}");

        if (currentPage < unlockedPages.Count - 1)
        {
            //unlockedPages[currentPage].SetActive(false); 

            currentPage++;

            //unlockedPages[currentPage].SetActive(true);
            Debug.Log($"Added to current page {currentPage}");
            
            
            UpdatePageVisibility();
        }
        Debug.Log($"End of next page");
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            //unlockedPages[currentPage].SetActive(false);

            currentPage--;

            //unlockedPages[currentPage].SetActive(true);
            //currentPage--;
            UpdatePageVisibility();
        }
    }


    public void UpdatePageVisibility()
    {


        
        for (int i = 0; i < unlockedPages.Count; i++)
        {
            bool check = i == currentPage; //Checks if i is current page?

            unlockedPages[i].SetActive(check); // If i is current page, set active
                                               // Show only the active page 
        }
        

    }
    //*
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Keypad0) )
        {
            AddJournalEntry(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddJournalEntry(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            AddJournalEntry(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            AddJournalEntry(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            AddJournalEntry(4);
        }        
     }
     


}





//using UnityEngine;
//using UnityEngine.UI;

//public class JournalUI : MonoBehaviour
//{
//    public static JournalUI Instance;
//    public GameObject journalPanel; // Reference to the journal UI 
//    public Button openJournalButton; // Button to open the journal
//    public Button closeJournalButton; // Button to close the journal
//    public Animator playerAnimator; //player animation

//    // [kr]
//    public GameObject[] journalPages; //Array of UI Panel (each panel is one page)
//    public Button nextPageButton; // Button to go to the next page
//    public Button prevPageButton; // Button to go to the previous page
//    private int currentPage = 0; // Tracks the currently visible page
//    private int totalPagesUnlocked = 0; // Number of unlocked pages
//    public GameObject notificationCircle; // UI element for the notification badge 
//    // [kr]

//    void Start()
//    {

//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Debug.Log("Journal UI not found");
//        }

//        // Hide journal at the start
//        journalPanel.SetActive(false);

//        // Add listeners to buttons
//        openJournalButton.onClick.AddListener(OpenJournal);
//        closeJournalButton.onClick.AddListener(CloseJournal);

//        // [kr]
//        // Hide all pages at the start
//        foreach (GameObject page in journalPages)
//        {
//            page.SetActive(false);
//            Debug.Log("Turn Off: " + page);
//        }


//        //  Add listeners to next/prev page buttons
//        nextPageButton.onClick.AddListener(NextPage);
//        prevPageButton.onClick.AddListener(PreviousPage);

//        notificationCircle.SetActive(false); // Hide notification initially
//        UpdatePageVisibility();


//        // [kr]
//    }

//    void OpenJournal()
//    {
//        journalPanel.SetActive(true);
//        /*JournalText.SetActive(false);*/
//        Time.timeScale = 0f; // **PAUSE GAME**

//        if (playerAnimator != null )
//        {
//            playerAnimator.enabled = false;
//        }

//        if(notificationCircle.active)
//        {
//            notificationCircle.SetActive(false);
//        }
//    }

//    public void CloseJournal()
//    {
//        journalPanel.SetActive(false);
//        Time.timeScale = 1f; // **RESUME GAME**

//        if (playerAnimator != null)
//        {
//            playerAnimator.enabled = true;
//        }
//    }

//    // [kr]

//    public void AddJournalEntry()
//    {
//        if (totalPagesUnlocked < journalPages.Length)
//        {
//            totalPagesUnlocked++;
//            journalPages[totalPagesUnlocked - 1].SetActive(true); // Unhide the next page            
//            currentPage = totalPagesUnlocked - 1;
//            UpdatePageVisibility();
//            notificationCircle.SetActive(true); // Show notification circle
//        }
//    }

//    public void NextPage()
//    {
//        if (currentPage < totalPagesUnlocked - 1)
//        {
//            currentPage++;
//            UpdatePageVisibility();
//        }
//    }

//    public void PreviousPage()
//    {
//        if (currentPage > 0)
//        {
//            currentPage--;
//            UpdatePageVisibility();
//        }
//    }

//    public void UpdatePageVisibility()
//    {
//        for (int i = 0; i < totalPagesUnlocked; i++)
//        {
//            //if i is the same as current page, "Add it to the journal"
//            journalPages[i].SetActive(i == currentPage); // Only show the active page
//        }
//    }

//    // [kr] 
//}
