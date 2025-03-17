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

        public ParcelData(/*GameObject parcelObject,*/ Vector2 position, int parcelID, string parcelName, Sprite parcelSprite,
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcList = FindAnyObjectByType<MainNpcs>().GetNPCList();
        AssignParcels();
        UpdateParcelVisibility();
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

    public void UpdateParcelVisibility()
    {
        Quest questManager = FindAnyObjectByType<Quest>();
        foreach (var parcel in assignedParcels)
        {
            if (parcel.parcelID < ParcelsObjects.Length)
            {
                GameObject parcelObj = ParcelsObjects[parcel.parcelID];
                Debug.LogError($"Parcel {parcel.parcelID} visibility set to: {parcel.isActiveQuest}");
                if (parcel.isActiveQuest)
                {

                    parcelObj.SetActive(true); // Show if quest is active
                }
                else
                {
                    parcelObj.SetActive(false); // Hide if quest is not active
                }
            }
            else
            {
                Debug.LogError($"Parcel ID {parcel.parcelID} is out of bounds for ParcelsObjects array.");
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
