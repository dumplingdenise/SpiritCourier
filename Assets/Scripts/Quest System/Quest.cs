using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{

    public int maxActiveSlot = 3;
    public static Quest Instance  { get; private set; }

    private List<questData> allQuest = new List<questData>();
    private List<questData> activeQuest = new List<questData>();
    private questData currentDisplayedQuest; // test code

    public Button[] taskSlots;
    public Image[] taskIcons;

    private GameObject questUI;

    [Header("Pop-up UI")]
    public GameObject hintPopup;
    public Image hintImage;
    public Text hintTitle;
    public Text hintText;
    public Button closeBtn;

    public enum questType 
    {
        findParcel,
        deliverParcel
    }

    public enum questStatus
    { 
        inActive,
        inProgress,
        completed
    }

    // test code
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject); // Prevent duplicate instances
        }
    }

    public class questData
    {
        public int questID;
        public questType questType;
        public questStatus questStatus;
        public Parcels.ParcelData ParcelData;
        public GameObject parcelObject;

        public questData(int questID, questType questType, questStatus questStatus, Parcels.ParcelData parcelData, GameObject parcelObject)
        {
            this.questID = questID;
            this.questType = questType;
            this.questStatus = questStatus;
            this.ParcelData = parcelData;
            this.parcelObject = parcelObject;
        }
    }

    public void addParcelDatatoQuest(questData questData)
    {
        allQuest.Add(questData);
        Debug.LogError($"Queue list count: {allQuest.Count}");
        Debug.Log($"Quest ID: {questData.questID}, quest status: {questData.questStatus}, quest type: {questData.questType}, questData: {questData.ParcelData} added to quest list");
    }

    // test code
    /*public int GetQuest()
    {
        return allQuest.Count;
    }*/

    // Method to fill up active quest slots (up to maxActiveSlot)
    public void FillActiveQuests()
    {
        foreach (var quest in allQuest)
        {
            if (activeQuest.Count >= maxActiveSlot)
                break; // Stop if max slots are filled

            if (quest.questStatus == questStatus.inActive)
            {
                quest.questStatus = questStatus.inProgress;
                activeQuest.Add(quest);

                if (quest.parcelObject != null)
                {
                    quest.parcelObject.SetActive(true);
                }
                Debug.Log($"Quest ID {quest.questID} activated.");
                Debug.Log($"Active list count: {activeQuest.Count}");
                foreach (var quests in activeQuest)
                {
                    Debug.LogError($"Quest ID: {quests.questID}, quest status: {quests.questStatus}, quest type: {quests.questType}, questData: {quests.ParcelData.parcelName}, {quests.ParcelData.parcelID}");
                }

                OnQuestUpdated(); // test code
            }
        }
        UpdateQuestUI();
    }

    public questData GetQuestByParcelID(int parcelID)
    {
        foreach (var quest in activeQuest) // Assuming questList holds all active quests
        {
            if (quest.ParcelData != null && quest.ParcelData.parcelID == parcelID)
            {
                return quest;
            }
        }
        return null;
    }

    public void completeQuest(int questID)
    {
        var quest = activeQuest.Find(q => q.questID == questID);
        if (quest != null)
        {
            activeQuest.Remove(quest);
            FillActiveQuests();
            UpdateQuestUI();

            Debug.Log($"Quest ID: {quest.questID}, Parcel {quest.ParcelData.parcelName}, ID: {quest.ParcelData.parcelID} successfully delivered. Removing from quest");

            // test code
            // Check if the completed quest is the one displayed, and close or update the popup
            if (currentDisplayedQuest == quest)
            {
                hintPopup.SetActive(false);
                currentDisplayedQuest = null;
            }
        }
    }

    // Method to update the quest UI (task icons & buttons)
    void UpdateQuestUI()
    {
        // Clear existing quest icons
        for (int i = 0; i < taskIcons.Length; i++)
        {
            taskIcons[i].sprite = null;
            taskIcons[i].gameObject.SetActive(false);
            taskSlots[i].gameObject.SetActive(true);
            taskSlots[i].onClick.RemoveAllListeners();

            // test code
            Image selectedImage = taskSlots[i].transform.Find("Selected").GetComponent<Image>();
            selectedImage.gameObject.SetActive(false);
        }

        // Populate quest icons
        for (int i = 0; i < activeQuest.Count; i++)
        {
            Debug.Log("Updating Quest Slot: " + activeQuest[i].ParcelData.parcelName);

            // Use the parcel sprite for all quests
            taskSlots[i].gameObject.SetActive(true);
            taskIcons[i].sprite = activeQuest[i].ParcelData.parcelSprite;
            taskIcons[i].gameObject.SetActive(true);

            // Add a click listener to show quest details
            int index = i; // Capture the index for the lambda
            taskSlots[i].onClick.AddListener(() => ShowQuestDetails(activeQuest[index], /*taskIcons[index]*/index));
        }
    }

    // Show quest details (with auto-update logic)
    private void ShowQuestDetails(questData quest, /*Image slotImage*/ int index)
    {
        Debug.Log("Showing quest hint");
        Debug.Log($"Quest ID: {quest.questID}, Type: {quest.questType}, Hint: {quest.ParcelData.parcelHints}");

        hintPopup.SetActive(true);
        currentDisplayedQuest = quest; // Track the displayed quest

        UpdateHintPopup();

        for (int i = 0; i < taskSlots.Length; i++)
        {
            Image selectedImage = taskSlots[i].transform.Find("Selected").GetComponent<Image>(); // Get the Image component of Selected
            selectedImage.gameObject.SetActive(i == index);  // Only activate the selected one
        }

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            hintPopup.SetActive(false);
            currentDisplayedQuest = null; // Clear when closed

            foreach (var taskSlot in taskSlots)
            {
                Image selectedImage = taskSlot.transform.Find("Selected").GetComponent<Image>();
                selectedImage.gameObject.SetActive(false);
            }

        });
    }

    // Method to update the hint UI dynamically when the quest changes
    private void UpdateHintPopup()
    {
        if (currentDisplayedQuest == null) return; // Skip if no quest is being displayed

        string hintTitleText = "";
        string hintMessage = "";

        if (currentDisplayedQuest.questType == questType.findParcel)
        {
            hintTitleText = $"Find {currentDisplayedQuest.ParcelData.parcelName}";
            hintMessage = $"{currentDisplayedQuest.ParcelData.parcelHints}";
        }
        else if (currentDisplayedQuest.questType == questType.deliverParcel)
        {
            hintTitleText = $"Deliver {currentDisplayedQuest.ParcelData.parcelName} to the right spirit";
            hintMessage = $"{currentDisplayedQuest.ParcelData.npcHints}";
        }

        hintTitle.text = hintTitleText;
        hintText.text = hintMessage;
        hintImage.sprite = currentDisplayedQuest.ParcelData.parcelSprite;
        hintImage.gameObject.SetActive(true);
    }

    // Call this whenever a quest status changes
    public void OnQuestUpdated()
    {
        if (hintPopup.activeSelf) // Only update if the popup is open
        {
            UpdateHintPopup();
        }
    }

    public void HideQuestUI()
    {
        questUI = this.gameObject;
        questUI.gameObject.SetActive(false);
    }

    public void ShowQuestUI()
    {
        questUI = this.gameObject;
        questUI.gameObject.SetActive(true);
    }

    public void Start()
    {
        /*Instance = this;*/
        hintPopup.SetActive(false);
        UpdateQuestUI();
    }
}

