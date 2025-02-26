using System.Collections;
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
        /*public GameObject parcelObject;*/
        public Vector2 position;
        public int parcelID;
        public string parcelName;
        public Sprite parcelSprite;
        public MainNpcs.NPCData assignedNpcData;
        public string parcelHints;
        public string npcHints;

        public ParcelData(/*GameObject parcelObject,*/ Vector2 position, int parcelID, string parcelName, Sprite parcelSprite, MainNpcs.NPCData assignedNpcData, string parcelHints, string npcHints) 
        {
            /*this.parcelObject = parcelObject;*/
            this.position = position;
            this.parcelID = parcelID;
            this.parcelName = parcelName;
            this.parcelSprite = parcelSprite;
            this.assignedNpcData = assignedNpcData;
            this.parcelHints = parcelHints;
            this.npcHints = npcHints;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcList = FindAnyObjectByType<MainNpcs>().GetNPCList();
        AssignParcels();

       /* // Make the parcel object hidden on start
        foreach (GameObject parcel in ParcelsObjects)
        {
            parcel.SetActive(false);
        }*/
    }

    void AssignParcels()
    {
        int parcelID = 0;
        Quest questManager = FindAnyObjectByType<Quest>();

        foreach (GameObject parcelObj in ParcelsObjects)
        {
            /*GameObject parcelObject = parcelObj;*/
            Sprite parcelSprite = parcelObj.GetComponent<SpriteRenderer>().sprite;
            string parcelName = parcelObj.name;

            PickUpParcel pickUpParcel = parcelObj.GetComponent<PickUpParcel>();

            string parcelHints = pickUpParcel.parcelHints;
            string npcHints = pickUpParcel.npcHints;
            
            if (pickUpParcel != null)
            {
                // Ensure that you have an NPC to assign to the parcel
                if (pickUpParcel.assignedNPC != null)
                {
                    ParcelData parcelData = new ParcelData(
                        /*parcelObject,*/
                        (Vector2)parcelObj.transform.position,
                        parcelID,
                        parcelName,
                        parcelSprite,
                        pickUpParcel.assignedNPC,
                        parcelHints,
                        npcHints);

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
