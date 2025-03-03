using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;
using static Parcels_Test;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class DeliverParcel : MonoBehaviour
{
    private bool playerNearby = false;

    private Label promptText;

    private List<Inventory.inventoryParcelData> inventoryList;

    private int NpcID;
    private string npcName;
    private string targetedNpcName;

    private Quest quest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainNpcs mainNpcs = FindFirstObjectByType<MainNpcs>();
        if (mainNpcs != null)
        {
            foreach (var npcData in mainNpcs.GetNPCList())
            {
                if (npcData.npcInstance == gameObject)
                {
                    NpcID = npcData.npcID;
                    npcName = npcData.npcInstance.name;
                    break;
                }
            }
        }

        inventoryList = FindAnyObjectByType<Inventory>().GetInventoryList();
        quest = FindAnyObjectByType<Quest>();
        var uiDocument = GetComponentInParent<UIDocument>();
        if (uiDocument != null)
        {
            var rootVisualElement = uiDocument.rootVisualElement; // Get the UIDocument in the spawned parcel gameobject.

            /*promptText = rootVisualElement.Q("DeliverLabel"); // get the Label*/

            promptText = rootVisualElement.Q<Label>("DeliverLabel");

            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.None; // do not display it on start
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (inventoryList.Count == 0)
            {
                Debug.Log("Nothing in inventory");
                if (promptText != null)
                {
                    promptText.style.display = DisplayStyle.Flex; // do not display the prompt when player come in contact with the NPC with no parcels in inventory
                    promptText.text = "You have no parcels to deliver! Go find some!!";
                }
            }
            else
            {
                playerNearby = true;
                Debug.Log("Player detected, choose a parcel to deliver");

                if (promptText != null)
                {
                    promptText.style.display = DisplayStyle.Flex; // display the prompt when player come in contact with the NPC with parcels in the inventory
                    promptText.text = "Choose a parcel to deliver to the spirit.";
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;

            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.None; // set display to none again when player walk away from the NPC
                promptText.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var inventory = FindFirstObjectByType<Inventory>();

        // Only allow delivery if the player is near an NPC
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!playerNearby)
            {
                return; // Exit to prevent parcel removal
            }
            else
            {
                // Check if a parcel is selected
                if (inventory.selectedSlot < 0 || inventory.selectedSlot >= inventory.GetInventoryList().Count)
                {
                    if (promptText != null)
                    {
                        promptText.style.display = DisplayStyle.Flex;
                        promptText.text = "Please select a parcel first!";
                    }
                }
                else
                {
                    var selectedParcel = inventory.GetInventoryList()[inventory.selectedSlot];

                    // Check if parcel matches the current NPC
                    if (selectedParcel.npcData.npcID == NpcID)
                    {
                        Debug.Log($"Parcel {selectedParcel.parcelName} delivered successfully to NPC {npcName}");
                        if (promptText != null)
                        {
                            promptText.text = $"{selectedParcel.parcelName} successfully delivered to {npcName}";
                            promptText.style.display = DisplayStyle.Flex;
                        }

                        StartCoroutine(DialogManager.Instance.ShowDialog(DialogManager.Instance.CreateSuccessDialog(selectedParcel.parcelName)));

                        // Remove parcel from inventory after successful delivery
                        inventory.RemoveParcelFromInventory();

                        if (quest != null)
                        {
                            quest.completeQuest(selectedParcel.parcelID);
                        }
                    }
                    else
                    {
                        Debug.Log("This parcel is for a different spirit!");
                        if (promptText != null)
                        {
                            promptText.text = "This parcel is not for me!";
                            promptText.style.display = DisplayStyle.Flex;

                            
                            Debug.Log($"parcel {selectedParcel.parcelName} is meant for {targetedNpcName} but you are giving to {npcName}, {NpcID}");
                        }

                        StartCoroutine(DialogManager.Instance.ShowDialog(DialogManager.Instance.CreateFailureDialog(/*targetedNpcName*/)));
                        
                        inventory.selectedSlot = -1;
                        inventory.UpdateInventoryUI(); // test code
                        
                    }
                }
            }
        }
    }
}
