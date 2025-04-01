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
    private bool playerNearby = false;

    private Label pickUpPromptText;

    private Inventory inventory;
    private Quest quest;
    public MainNpcs.NPCData assignedNPC;
    public Parcels.ParcelData parcelData; // get the parcel info from Parcels script
    public string Tag;
    public string parcelHints;
    public string npcHints;
    public Dialog parcelStoryDialog;

    private bool pickedUp = false;

    public GameObject indicator;

    private void Start()
    {

        inventory = GameObject.FindFirstObjectByType<Inventory>();
        quest = GameObject.FindFirstObjectByType<Quest>();
        
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
        else
        {
            return;
        }

        var uiDocument = GetComponentInParent<UIDocument>();
        if (uiDocument != null)
        {
            var rootVisualElement = uiDocument.rootVisualElement;
            pickUpPromptText = rootVisualElement.Q<Label>("PickUpLabel");
            /* if (pickUpPromptText != null)
             {
                 pickUpPromptText.style.display = DisplayStyle.None;
                 Debug.Log("Prompt text is not here!");
             }*/

            if (pickUpPromptText == null)
            {
                Debug.LogError("PickUpLabel not found in UIDocument! Check the label name.");
            }
            else
            {
                Debug.LogError("PickUpLabel found successfully! Current display: " + pickUpPromptText.style.display);

                // Try forcing it again
                pickUpPromptText.style.display = DisplayStyle.None;
                Debug.LogError("After hiding: " + pickUpPromptText.style.display);
            }
        }
        else
        {
            Debug.LogError("UIDocument is missing or not attached to this object.");
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
                Invoke(nameof(HidePrompt), 1f);
            }
            else
            {
                Debug.LogError("Prompt text is not found!");
            }


            /*if (indicatorInstance == null && indicator != null)
            {
                indicator.SetActive(true);
                *//*Debug.LogError("Parcel Position: " + parcelData.position);
                indicatorInstance = Instantiate(indicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                indicatorInstance.transform.SetParent(transform, true); // Attach to parcel*//*
                UpdateIndicatorPosition();
            }
            Debug.LogError($"Parcel location: {transform.position}");*/

            /*if (indicatorInstance == null && indicator != null)
            {
                indicatorInstance = Instantiate(indicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                indicatorInstance.transform.SetParent(transform, true); // Attach to the parcel object
                UpdateIndicatorPosition();
                indicator.SetActive(true);
            }*/

            if (indicator != null && playerNearby)
            {
                UpdateIndicatorPosition();
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

           if (indicator != null)
            {
                indicator.SetActive(false);
            }
        }
    }

    private void HidePrompt()
    {
        if (pickUpPromptText != null)
        {
            pickUpPromptText.style.display = DisplayStyle.None;
            pickUpPromptText.text = "";
        }
    }


    private void UpdateIndicatorPosition()
    {
        if (indicator != null)
        {
            indicator.transform.position = transform.position + new Vector3(0, 1f, 0); // Slightly above the parcel
            indicator.SetActive(true);
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

                //Debug.Log($"Attempting to add parcel: ID={parcelData.parcelID}, Position={parcelData.position}, Sprite={parcelData.parcelSprite}, Assigned NPC: {parcelData.assignedNpcData}");

                bool added = inventory.AddParcelToInventory(parcelData.parcelID, parcelData.parcelName, parcelData.parcelSprite, parcelData.position, parcelData.assignedNpcData, parcelData.parcelStoryDialog, parcelData.tag); // add to inventory
                inventory.parcelPickedUp++;

                if (added)
                {
                    SoundEffectManager.Play("PickUpParcels");
                    Debug.Log("Parcel picked up!");

                    /*if (pickUpPromptText != null)
                    {
                        pickUpPromptText.style.display = DisplayStyle.Flex;  // if added to inventory, text is gone
                        pickUpPromptText.text = $"{parcelData.parcelName} picked up and added to inventory!";
                        Invoke(nameof(HidePrompt), 3f);
                        Debug.Log($"Parcel {parcelData.parcelName}, ID: {parcelData.parcelID} picked up and added to inventory!");
                    }*/
                    inventory.UpdateInventoryUI();

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
                    
                   /* PlayerPrefs.SetInt($"Parcel_{parcelData.parcelID}_PickedUp", 1);*/ // test code
                    Invoke(nameof(destroyParcel), 0.1f);
                }
                else
                {
                    Debug.Log("Inventory full, cannot pick up the parcel.");
                    if (pickUpPromptText != null)
                    {
                        pickUpPromptText.style.display = DisplayStyle.Flex;  // if added to inventory, text is gone

                        pickUpPromptText.text = "Inventory full, can't pick up the parcel!";
                        Invoke(nameof(HidePrompt), 3f);
                    }
                }
            }
        }
    }

    private void destroyParcel()
    {
        gameObject.SetActive(false); // Hide the parcel
        Destroy(gameObject); 
    }
}
