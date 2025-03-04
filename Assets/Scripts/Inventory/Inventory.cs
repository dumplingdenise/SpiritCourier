//using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    public int maxInventorySize = 5;
    public int parcelPickedUp = 0;
    private List<inventoryParcelData> collectedParcels = new List<inventoryParcelData>();
    /*private List<MainNpcs.NPCData> assignedNpcs = new List<MainNpcs.NPCData>();*/ // To store linked NPCs

    public Image[] slotIcons;
    public Button[] slotButtons; // Array of buttons for each inventory slot

    public int selectedSlot = -1;

    public class inventoryParcelData
    {
        public int parcelID;
        public string parcelName;
        public Sprite parcelSprite;
        public Vector2 parcelPosition;
        public MainNpcs.NPCData npcData;
        public Dialog parcelStoryDialog;


        public inventoryParcelData(int parcelID, string parcelName, Sprite parcelSprite, Vector2 parcelPosition, MainNpcs.NPCData npcData, Dialog parcelStoryDialog)
        {
            this.parcelID = parcelID;
            this.parcelName = parcelName;
            this.parcelSprite = parcelSprite;
            this.parcelPosition = parcelPosition;
            this.npcData = npcData;
            this.parcelStoryDialog = parcelStoryDialog;
        }
    }

    public List<inventoryParcelData> GetInventoryList()
    {
        return collectedParcels;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateInventoryUI();

        // Add listeners to each button for selection
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;
            slotButtons[i].onClick.AddListener(() => OnSlotClick(index));
        }
    }

    public bool AddParcelToInventory(int parcelID, string parcelName, Sprite parcelSprite, Vector2 parcelPosition, MainNpcs.NPCData npcData, Dialog parcelStoryDialog)
    {
        if (collectedParcels.Count >= maxInventorySize)
        {
            Debug.Log("Inventory is full");
            return false;
        }

        inventoryParcelData newParcel = new inventoryParcelData(parcelID, parcelName, parcelSprite, parcelPosition, npcData, parcelStoryDialog);
        collectedParcels.Add(newParcel);


        if (parcelStoryDialog != null && parcelStoryDialog.Lines != null)
        {
            foreach (var line in parcelStoryDialog.Lines)
            {
                Debug.Log($"Parcel Name: {parcelName}, Parcel ID: {parcelID} added to inventory at position {parcelPosition}. Assigned to NPC ID: {npcData.npcID} with following storyline: {line}");
            }
        }
        // Debug.Log($"Parcel Name: {parcelName}, Parcel ID {parcelID} added to inventory at position {parcelPosition}. Assigned to NPC ID: {npcData.npcID} with following storyline: {parcelStoryDialog}");

        UpdateInventoryUI();
        return true;
    }
    public void OnSlotClick(int index)
    {
        // Set the selected slot to the clicked index
        if (index < collectedParcels.Count)
        {
            selectedSlot = index;
            Debug.Log($"Selected slot: {selectedSlot}");

            // test code for selected item in inventory -> but if the player deliver a wrong parcel, the blue bg is still there
            for (int i = 0; i < slotButtons.Length; i++)
            {
                Image selectedImage = slotButtons[i].transform.Find("Selected").GetComponent<Image>();
                selectedImage.gameObject.SetActive(i == selectedSlot);
            }
        }
        // test code
        else
        {
            // Reset selected slot if it's out of bounds
            selectedSlot = -1;

            // Ensure all blue backgrounds are deactivated
            for (int i = 0; i < slotButtons.Length; i++)
            {
                Image selectedImage = slotButtons[i].transform.Find("Selected").GetComponent<Image>();
                selectedImage.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveParcelFromInventory()
    {
        /*for (int i = 0; i < collectedParcels.Count; i++)
        {
            if (collectedParcels[i].ParcelID == parcelID)
            {
                collectedParcels.RemoveAt(i);
                Debug.Log($"Parcel {parcelID} removed from inventory.");
                UpdateInventoryUI();
                return;
            }
        }
        Debug.Log("Parcel not found in inventory.");*/

        if (selectedSlot >= 0 && selectedSlot < collectedParcels.Count)
        {
            collectedParcels.RemoveAt(selectedSlot);
            Debug.Log($"Parcel at slot {selectedSlot} removed from inventory.");
            selectedSlot = -1; // Reset selection

            //test code
            for (int i = 0; i < slotButtons.Length; i++)
            {
                Image selectedImage = slotButtons[i].transform.Find("Selected").GetComponent<Image>();
                selectedImage.gameObject.SetActive(false);
            }

            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("No parcel selected for removal.");
        }
    }

    public void UpdateInventoryUI()
    {
        for (int i = collectedParcels.Count - 1; i >= 0; i--)
        {
            if (collectedParcels[i] == null)
            {
                Debug.LogError($"Parcel at index {i} is null or destroyed, removing from inventory");
                collectedParcels.RemoveAt(i);
            }
        }

        // Now update the UI normally
        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (i < collectedParcels.Count && collectedParcels[i] != null)
            {
                slotIcons[i].sprite = collectedParcels[i].parcelSprite;
                slotIcons[i].enabled = true;
            }
            else
            {
                slotIcons[i].sprite = null;
                slotIcons[i].enabled = false;
            }

            // Reset the selection background for all slots
            Image selectedImage = slotButtons[i].transform.Find("Selected").GetComponent<Image>();
            selectedImage.gameObject.SetActive(i == selectedSlot);
        }

        Debug.Log($"Inventory updated. Total parcels: {collectedParcels.Count}");
    }


    // Update is called once per frame
    void Update()
    {
    }
}
