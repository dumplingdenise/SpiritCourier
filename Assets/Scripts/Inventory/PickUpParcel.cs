//using Mono.Cecil.Cil;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Quest;

public class PickUpParcel : MonoBehaviour
{
    // code below here is when player walk into the parcel,  they have to press E to pick up

    private bool playerNearby = false;

    private Label pickUpPromptText;

    private Inventory inventory;
    private Quest quest;
    private Parcels parcel;
    public MainNpcs.NPCData assignedNPC;
    public Parcels.ParcelData parcelData; // get the parcel info from Parcels script
    public string parcelHints;
    public string npcHints;
    public Dialog parcelStoryDialog;

    private bool pickedUp = false;

    private void Start()
    {
        inventory = GameObject.FindFirstObjectByType<Inventory>();
        quest = GameObject.FindFirstObjectByType<Quest>();
        parcel = GameController.FindFirstObjectByType<Parcels>();

        var uiDocument = GetComponentInParent<UIDocument>();
        if (uiDocument != null)
        {
            var rootVisualElement = uiDocument.rootVisualElement;
            pickUpPromptText = rootVisualElement.Q<Label>("PickUpLabel");
            if (pickUpPromptText != null)
            {
                pickUpPromptText.style.display = DisplayStyle.None;
            }
        }

        StartCoroutine(WaitForNpcAssignment());
    }

    private IEnumerator WaitForNpcAssignment()
    {
        // Wait until assignedNpcData is set
        yield return new WaitUntil(() => parcelData != null && parcelData.assignedNpcData != null);

        assignedNPC = parcelData.assignedNpcData;

        if (assignedNPC != null)
        {
            Debug.Log($"Parcel Name: {parcelData.parcelName} Parcel ID: {parcelData.parcelID}, Parcel Hint: {parcelData.parcelHints} assigned to: {assignedNPC.npcID}");
        }
        else
        {
            Debug.LogError("No NPC assigned to the parcel.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Parcel detected! Press 'E' to pick up.");

            if (pickUpPromptText != null)
            {
                pickUpPromptText.style.display = DisplayStyle.Flex; // display the prompt when player come in contact with the parcel
                pickUpPromptText.text = "Press 'E' to pick up parcel.";
                StartCoroutine(HidePrompt(2f));  // Hides the prompt after 2 seconds
            }
            else
            {
                Debug.LogError("Prompt text is not found!");
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;

            if (pickUpPromptText != null)
            {
                pickUpPromptText.style.display = DisplayStyle.None; // set display to none again when player walk away from the parcel
            }
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !pickedUp)
        {
            if (inventory != null)
            {
                if (parcelData == null) // check if parcelData is empty
                {
                    Debug.LogError("Parcel Data is not assigned!");
                    return;
                }

                bool added = inventory.AddParcelToInventory(parcelData.parcelID, parcelData.parcelName, parcelData.parcelSprite, parcelData.position, parcelData.assignedNpcData, parcelData.parcelStoryDialog); // add to inventory
                inventory.parcelPickedUp++;

                if (added)
                {
                    pickedUp = true;
                    
                    Debug.Log("Parcel picked up!");

                    if (pickUpPromptText != null)
                    {
                        pickUpPromptText.style.display = DisplayStyle.Flex;  // if added to inventory, text is gone
                        pickUpPromptText.text = $"{parcelData.parcelName} picked up and added to inventory!";
                        StartCoroutine(HidePrompt(2f));  // Hides the prompt after 2 seconds
                        Debug.Log($"Parcel {parcelData.parcelName}, ID: {parcelData.parcelID} picked up and added to inventory!");
                    }
                    inventory.UpdateInventoryUI();

                    // test
                    if (parcel != null)  // Call MarkParcelPicked here
                    {
                        parcel.MarkParcelPicked(parcelData.parcelID);
                    }
                    else
                    {
                        Debug.LogError("Parcels Instance not found!");
                    }

                    if (quest != null)
                    {
                        questData currentQuest = quest.GetQuestByParcelID(parcelData.parcelID);
                        if (currentQuest != null)
                        {
                            currentQuest.questType = questType.deliverParcel;
                            Debug.Log($"Quest updated to Deliver Parcel for Parcel ID: {parcelData.parcelID}");
                        }
                        else
                        {
                            Debug.LogError("No quest found for this parcel.");
                        }
                        quest.OnQuestUpdated();
                    }

                    Invoke(nameof(destroyParcel), 0.1f);
                }
                else
                {
                    Debug.Log("Inventory full, cannot pick up the parcel.");
                    if (pickUpPromptText != null)
                    {
                        pickUpPromptText.style.display = DisplayStyle.Flex;  // if added to inventory, text is gone
                        pickUpPromptText.text = "Inventory full, can't pick up the parcel!";
                        StartCoroutine(HidePrompt(2f));  // Hides the prompt after 2 seconds
                    }
                }
            }
        }
    }

    IEnumerator HidePrompt(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickUpPromptText.style.display = DisplayStyle.None;
    }

    private void destroyParcel()
    {
        /*gameObject.SetActive(false); // Hide the parcel*/
        Destroy(gameObject); 
    }
}
