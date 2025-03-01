using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Quest;

public class Quest : MonoBehaviour
{

    public int maxActiveSlot = 3;

    private List<questData> allQuest = new List<questData>();
    private List<questData> activeQuest = new List<questData>();

    public Button[] taskSlots;
    public Image[] taskIcons;

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
        Debug.Log($"Quest ID: {questData.questID}, quest status: {questData.questStatus}, quest type: {questData.questType}, questData: {questData.ParcelData} added to quest list");
    }

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

                UpdateQuestUI();
                Debug.Log($"Quest ID {quest.questID} activated.");
            }
        }
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
            /*if (quest.parcelObject != null)
            {
                quest.parcelObject.SetActive(false);
            }*/
            activeQuest.Remove(quest);
            FillActiveQuests();

            Debug.Log($"Parcel {quest.ParcelData.parcelName}, ID: {quest.ParcelData.parcelID} successfully delivered. Removing from quest");
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
            taskSlots[i].onClick.AddListener(() => ShowQuestDetails(activeQuest[index]));
        }
    }

    // Show quest details (e.g., in a popup or log)
    private void ShowQuestDetails(questData quest)
    {
        Debug.Log($"Quest ID: {quest.questID}, Type: {quest.questType}, Hint: {quest.ParcelData.parcelHints}");
        
        hintPopup.SetActive(true);

        string hintMessage = "";
        string hintTitleText = "";

        if (quest.questType == questType.findParcel)
        {
            hintTitleText = $"Find {quest.ParcelData.parcelName}";
            hintMessage = $"{quest.ParcelData.parcelHints}";
        }
        else if (quest.questType == questType.deliverParcel)
        {
            hintTitleText = $"Deliver {quest.ParcelData.parcelName} to the right spirit";
            hintMessage = $"{quest.ParcelData.npcHints}";
        }
        hintTitle.text = hintTitleText;
        hintText.text = hintMessage;

        hintImage.sprite = quest.ParcelData.parcelSprite;
        hintImage.gameObject.SetActive(true);

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() => hintPopup.SetActive(false));
    }
    public void Start()
    {
        hintPopup.SetActive(false);
        UpdateQuestUI();
    }
}

/* public int maxQuestSize = 3; // Max number of active quests
     private List<questData> activeQuests = new List<questData>();
     private Queue<questData> pendingQuests = new Queue<questData>();

     public Button[] taskSlots;
     public Image[] taskImages;

     public Parcels parcelScript;
     public enum questType
     {
         findParcel,
         deliverParcel
     }

     public class questData
     {
         public int questID;
         public questType questType;
         public Parcels.ParcelData parcelData;
         public GameObject parcelObject;
         public bool isCompleted;

         public questData(int id, questType type, Parcels.ParcelData data, GameObject parcelObj)
         {
             this.questID = id;
             this.questType = type;
             this.parcelData = data;
             this.parcelObject = parcelObj;
             this.isCompleted = false;
         }

         public string getHint()
         {
             return questType == questType.findParcel ? parcelData.parcelHints : parcelData.npcHints;
         }
     }

     public void AddQuest(questData newQuest)
     {
         if (activeQuests.Count < maxQuestSize)
         {
             activeQuests.Add(newQuest);
             ActivateParcel(newQuest); // Only activate if it's in active quests
             Debug.Log("Active Quest Added: " + newQuest.parcelData.parcelName);
         }
         else
         {
             pendingQuests.Enqueue(newQuest); // Add extra quests to the queue
             Debug.Log("Quest added to pending queue: " + newQuest.parcelData.parcelName);
         }

         UpdateQuestUI();
     }

     // Complete a quest
     *//*    public void CompleteQuest(int questID)
         {
             questData quest = activeQuests.Find(q => q.questID == questID);
             if (quest != null)
             {
                 quest.isCompleted = true;
                 activeQuests.Remove(quest);
                 DeactivateParcel(quest);

                 // Add the next pending quest to active quests
                 if (pendingQuests.Count > 0)
                 {
                     questData nextQuest = pendingQuests.Dequeue();
                     activeQuests.Add(nextQuest);
                     ActivateParcel(nextQuest);
                 }

                 UpdateQuestUI();
             }
             else
             {
                 Debug.LogWarning($"Quest with ID {questID} not found.");
             }
         }*//*

     public void CompleteQuest(int questID)
     {
         questData quest = activeQuests.Find(q => q.questID == questID);
         if (quest != null)
         {
             quest.isCompleted = true;
             activeQuests.Remove(quest);
             DeactivateParcel(quest);

             // Activate next pending quest
             if (pendingQuests.Count > 0)
             {
                 questData nextQuest = pendingQuests.Dequeue();
                 activeQuests.Add(nextQuest);
                 ActivateParcel(nextQuest);
                 Debug.Log("Pending Quest Activated: " + nextQuest.parcelData.parcelName);
             }

             UpdateQuestUI();
         }
         else
         {
             Debug.LogWarning($"Quest with ID {questID} not found.");
         }
     }

     // Activate the parcel GameObject
     private void ActivateParcel(questData quest)
     {
         *//*if (quest.parcelData != null && quest.parcelData.parcelObject != null)
         {
             quest.parcelData.parcelObject.SetActive(true);
             Debug.Log($"Parcel {quest.parcelData.parcelName} activated!");
         }*//*

         Debug.Log($"Activating parcel: {quest.parcelData.parcelName}");
         parcelScript.ActivateParcelByName(quest.parcelData.parcelName); // Call the method from Parcels script
     }

     // Deactivate the parcel GameObject
     private void DeactivateParcel(questData quest)
     {
         if (quest.parcelObject != null)
         {
             quest.parcelObject.SetActive(false);
         }
     }

     // Update the quest UI
     private void UpdateQuestUI()
     {
         Debug.Log("Active Quests Count: " + activeQuests.Count);
         // Clear existing quest icons
         for (int i = 0; i < taskImages.Length; i++)
         {
             taskImages[i].sprite = null;
             taskImages[i].gameObject.SetActive(false);
             taskSlots[i].gameObject.SetActive(true);
             taskSlots[i].onClick.RemoveAllListeners();
         }

         // Populate quest icons
         for (int i = 0; i < activeQuests.Count; i++)
         {
             Debug.Log("Updating Quest Slot: " + activeQuests[i].parcelData.parcelName);

             // Use the parcel sprite for all quests
             taskSlots[i].gameObject.SetActive(true);
             taskImages[i].sprite = activeQuests[i].parcelData.parcelSprite;
             taskImages[i].gameObject.SetActive(true);

             // Add a click listener to show quest details
             int index = i; // Capture the index for the lambda
             taskSlots[i].onClick.AddListener(() => ShowQuestDetails(activeQuests[index]));
         }
     }

     // Show quest details (e.g., in a popup or log)
     private void ShowQuestDetails(questData quest)
     {
         Debug.Log($"Quest ID: {quest.questID}, Type: {quest.questType}, Hint: {quest.getHint()}");
         // You can expand this to display details in a UI panel
     }
     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
     {
         foreach (var taskSlot in taskSlots)
         {
             taskSlot.gameObject.SetActive(true);
         }

         UpdateQuestUI();
     }

     // Update is called once per frame
     void Update()
     {

     }*/