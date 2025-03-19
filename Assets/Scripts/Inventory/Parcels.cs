/*using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Parcels : MonoBehaviour
{
    public GameObject[] ParcelsObjects;
    private List<ParcelData> assignedParcels = new List<ParcelData>();
    private List<MainNpcs.NPCData> npcList; // declares a private list to store NPC data

    [System.Serializable]
    public class ParcelData
    {
        *//*public GameObject parcelObject;*//*
        public Vector2 position;
        public int parcelID;
        public string parcelName;
        public Sprite parcelSprite;
        public MainNpcs.NPCData assignedNpcData;
        public string parcelHints;
        public string npcHints;
        // test code
        public Dialog parcelStoryDialog;

        public ParcelData(*//*GameObject parcelObject,*//* Vector2 position, int parcelID, string parcelName, Sprite parcelSprite, MainNpcs.NPCData assignedNpcData, string parcelHints, string npcHints, Dialog parcelStoryDialog) 
        {
            *//*this.parcelObject = parcelObject;*//*
            this.position = position;
            this.parcelID = parcelID;
            this.parcelName = parcelName;
            this.parcelSprite = parcelSprite;
            this.assignedNpcData = assignedNpcData;
            this.parcelHints = parcelHints;
            this.npcHints = npcHints;
            this.parcelStoryDialog = parcelStoryDialog; // test code
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcList = FindAnyObjectByType<MainNpcs>().GetNPCList();
        AssignParcels();

       *//* // Make the parcel object hidden on start
        foreach (GameObject parcel in ParcelsObjects)
        {
            parcel.SetActive(false);
        }*//*
    }

    void AssignParcels()
    {
        int parcelID = 0;
        Quest questManager = FindAnyObjectByType<Quest>();

        foreach (GameObject parcelObj in ParcelsObjects)
        {
            *//*GameObject parcelObject = parcelObj;*//*
            Sprite parcelSprite = parcelObj.GetComponent<SpriteRenderer>().sprite;
            string parcelName = parcelObj.name;

            PickUpParcel pickUpParcel = parcelObj.GetComponent<PickUpParcel>();

            string parcelHints = pickUpParcel.parcelHints;
            string npcHints = pickUpParcel.npcHints;
            Dialog parcelStorydialog = pickUpParcel.parcelStoryDialog; // test code
            
            if (pickUpParcel != null)
            {
                // Ensure that you have an NPC to assign to the parcel
                if (pickUpParcel.assignedNPC != null)
                {
                    ParcelData parcelData = new ParcelData(
                        *//*parcelObject,*//*
                        (Vector2)parcelObj.transform.position,
                        parcelID,
                        parcelName,
                        parcelSprite,
                        pickUpParcel.assignedNPC,
                        parcelHints,
                        npcHints,
                        parcelStorydialog // test code
                        );

                    assignedParcels.Add(parcelData);

                    // Pass the assigned NPC data to the PickUpParcel component
                    pickUpParcel.parcelData = parcelData;
                    
                    Quest.questData newQuest = new Quest.questData(parcelID, Quest.questType.findParcel, Quest.questStatus.inActive, parcelData, parcelObj);
                    questManager.addParcelDatatoQuest(newQuest);

                    parcelObj.SetActive(false);

                    Debug.Log($"Parcel Name: {parcelName}, Parcel ID: {parcelID}, Sprite: {parcelSprite} at {parcelObj.transform.position} assigned to NPC ID: {pickUpParcel.assignedNPC.npcID}");
                }
                else
                {
                    Debug.LogError($"No NPC assigned to parcel at {parcelObj.transform.position}");
                }

                parcelID++;
            }
            else
            {
                Debug.LogError($"No PickUpParcel component found on parcel at {parcelObj.transform.position}");
            }
        }
        questManager.FillActiveQuests();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using static Quest;

public class Parcels : MonoBehaviour
{
    public GameObject[] ParcelsObjects;
    private List<ParcelData> assignedParcels = new List<ParcelData>();
    private List<MainNpcs.NPCData> npcList; // declares a private list to store NPC data
    private List<int> pickedUpParcelIDs = new List<int>();

    // test code
    public static Parcels Instance { get; private set; }


    [System.Serializable]
    public class ParcelData
    {
        /*public GameObject parcelObject;*/
        public Vector2 position;
        public int parcelID;
        public string parcelName;
        public Sprite parcelSprite;
        public MainNpcs.NPCData assignedNpcData;
        public string parcelHints;
        public string npcHints;
        public Dialog parcelStoryDialog;
        // test
        public bool isActiveQuest;

        public ParcelData(Vector2 position, int parcelID, string parcelName, Sprite parcelSprite,
            MainNpcs.NPCData assignedNpcData, string parcelHints, string npcHints, Dialog parcelStoryDialog, bool isActiveQuest)
        {
            this.position = position;
            this.parcelID = parcelID;
            this.parcelName = parcelName;
            this.parcelSprite = parcelSprite;
            this.assignedNpcData = assignedNpcData;
            this.parcelHints = parcelHints;
            this.npcHints = npcHints;
            this.parcelStoryDialog = parcelStoryDialog;
            this.isActiveQuest = isActiveQuest;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps the parent object with all parcels
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicate instances
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcList = FindAnyObjectByType<MainNpcs>().GetNPCList();
        AssignParcels();
        UpdateParcelVisibility();

        // test code
        /*npcList = FindAnyObjectByType<MainNpcs>().GetNPCList();
        LoadPickedUpParcels(); // Load picked-up parcels when scene loads
        AssignParcels();
        Quest.Instance.FillActiveQuests(); // Force refresh active quests
        UpdateParcelVisibility();*/

    }

    void AssignParcels()
    {
        int parcelID = 0;
        Quest questManager = FindAnyObjectByType<Quest>();

        foreach (GameObject parcelObj in ParcelsObjects)
        {
            Sprite parcelSprite = parcelObj.GetComponent<SpriteRenderer>().sprite;
            string parcelName = parcelObj.name;

            PickUpParcel pickUpParcel = parcelObj.GetComponent<PickUpParcel>();

            string parcelHints = pickUpParcel.parcelHints;
            string npcHints = pickUpParcel.npcHints;
            Dialog parcelStorydialog = pickUpParcel.parcelStoryDialog;

            if (pickUpParcel != null)
            {
                // Ensure that you have an NPC to assign to the parcel
                if (pickUpParcel.assignedNPC != null)
                {
                    // test
                    // Check if this parcel is part of an active quest
                    bool isActiveQuest = questManager.IsQuestActive(parcelID); // Check if this parcel is assigned to an active quest
                    Debug.LogError($"ParcelID: {parcelID}, IsActiveQuest: {isActiveQuest}");

                    ParcelData parcelData = new ParcelData(
                        (Vector2)parcelObj.transform.position,
                        parcelID,
                        parcelName,
                        parcelSprite,
                        pickUpParcel.assignedNPC,
                        parcelHints,
                        npcHints,
                        parcelStorydialog,
                        // test
                        isActiveQuest
                        );
                    assignedParcels.Add(parcelData);


                    // Pass the assigned NPC data to the PickUpParcel component
                    pickUpParcel.parcelData = parcelData;

                    // Check if parcelID already exists in the quest list
                    if (!questManager.IsParcelInQuest(parcelID))
                    {
                        Quest.questData newQuest = new Quest.questData(parcelID, Quest.questType.findParcel, Quest.questStatus.inActive, parcelData, parcelObj, isActiveQuest);
                        questManager.addParcelDatatoQuest(newQuest);
                        Debug.LogError($"Parcel ID {parcelID} adding to quest");
                    }
                    else
                    {
                        Debug.LogError($"Parcel ID {parcelID} already exists in the quest list. Skipping...");
                    }

                    Debug.Log($"Parcel Name: {parcelName}, Parcel ID: {parcelID}, Sprite: {parcelSprite} at {parcelObj.transform.position} assigned to NPC ID: {pickUpParcel.assignedNPC.npcID}");
                }
                else
                {
                    Debug.LogError($"No NPC assigned to parcel at {parcelObj.transform.position}");
                }

                parcelID++;
            }
            else
            {
                Debug.LogError($"No PickUpParcel component found on parcel at {parcelObj.transform.position}");
            }
        }
        questManager.FillActiveQuests();
    }

    // test code
    public void MarkParcelPicked(int parcelID)
    {
        if (!pickedUpParcelIDs.Contains(parcelID))
        {
            pickedUpParcelIDs.Add(parcelID);
            Debug.Log($"Parcel {parcelID} picked up.");
        }
    }


    /*    public void MarkParcelPicked(int parcelID)
        {
            if (!pickedUpParcelIDs.Contains(parcelID))
            {
                pickedUpParcelIDs.Add(parcelID);
                SavePickedUpParcels();  // Save to persistence
                Debug.Log($"Parcel {parcelID} picked up.");
            }
        }

        private void SavePickedUpParcels()
        {
            PlayerPrefs.SetString("PickedUpParcels", string.Join(",", pickedUpParcelIDs));
            PlayerPrefs.Save();
        }

        private void LoadPickedUpParcels()
        {
            if (PlayerPrefs.HasKey("PickedUpParcels"))
            {
                string savedIDs = PlayerPrefs.GetString("PickedUpParcels");
                pickedUpParcelIDs = savedIDs.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();
            }
        }*/

    /*public void UpdateParcelVisibility()
    {
        Inventory inventoryManager = FindAnyObjectByType<Inventory>();
        List<int> pickedUpParcelIDs = new List<int>();

        foreach (var parcelData in inventoryManager.GetInventoryList())
        {
            pickedUpParcelIDs.Add(parcelData.parcelID);
        }


        foreach (var parcel in assignedParcels)
        {
            if (parcel.parcelID < ParcelsObjects.Length)
            {
                GameObject parcelObj = ParcelsObjects[parcel.parcelID];
                Debug.LogError($"Parcel {parcel.parcelID} visibility set to: {parcel.isActiveQuest}");
                // If parcel has been picked up already, don't show it again
                if (pickedUpParcelIDs.Contains(parcel.parcelID))
                {
                    parcelObj.SetActive(false); // Hide it if already in the inventory
                }
                else
                {
                    if (parcel.isActiveQuest)
                    {
                        parcelObj.SetActive(true); // Show if quest is active and not already picked up
                    }
                    else
                    {
                        parcelObj.SetActive(false); // Hide if quest is not active
                    }
                }
            }
            else
            {
                Debug.LogError($"Parcel ID {parcel.parcelID} is out of bounds for ParcelsObjects array.");
            }
        }
    }*/
    public void UpdateParcelVisibility()
    {
        foreach (var parcel in assignedParcels)
        {
            if (parcel.parcelID < ParcelsObjects.Length)
            {
                GameObject parcelObj = ParcelsObjects[parcel.parcelID];
                Debug.LogError($"Parcel {parcel.parcelID} visibility set to: {parcel.isActiveQuest}");
                // If parcel has been picked up already, don't show it again
                if (pickedUpParcelIDs.Contains(parcel.parcelID))
                {
                    parcelObj.SetActive(false); // Hide it if already in the inventory
                }
                else
                {
                    if (parcel.isActiveQuest)
                    {
                        parcelObj.SetActive(true); // Show if quest is active and not already picked up
                    }
                    else
                    {
                        parcelObj.SetActive(false); // Hide if quest is not active
                    }
                }
            }
            else
            {
                Debug.LogError($"Parcel ID {parcel.parcelID} is out of bounds for ParcelsObjects array.");
            }
        }
    }

    /*public void UpdateParcelVisibility()
    {
        foreach (var parcel in assignedParcels)
        {
            if (parcel.parcelID < ParcelsObjects.Length)
            {
                GameObject parcelObj = ParcelsObjects[parcel.parcelID];
                if (pickedUpParcelIDs.Contains(parcel.parcelID))
                {
                    parcelObj.SetActive(false);
                }
                else
                {
                    parcelObj.SetActive(parcel.isActiveQuest);
                }
            }
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        // test code
        /*UpdateParcelVisibility();*/
    }
}