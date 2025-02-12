using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PickUpParcel : MonoBehaviour
{

    // Code below is to when player walked into the parcel it will automatically pickup

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        Debug.Log($"Collision detected with: {collision.name}");
        if (player != null)
        {
            Debug.Log("Parcel collected by player!");
            Destroy(gameObject);
            player.parcelCollected++;
        }
    }*/



    // code below here is when player walk into the parcel,  they have to press E to pick up

    private bool playerNearby = false;

    private Label promptText;

    private Inventory inventory;
    public Parcels.ParcelData parcelData; // get the parcel info from Parcels script

    private bool pickedUp = false;

    private void Start()
    {
        inventory = GameObject.FindFirstObjectByType<Inventory>();
        var uiDocument = GetComponentInParent<UIDocument>();
        if (uiDocument != null)
        {
            var rootVisualElement = uiDocument.rootVisualElement; // Get the UIDocument in the spawned parcel gameObject.

            promptText = rootVisualElement.Q<Label>("PickUpLabel"); // get the Label
            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.None; // do not display it on start
            }
        }

        /*parcelPrompt = GetComponent<Text>();*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Parcel detected! Press 'E' to pick up.");

            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.Flex; // display the prompt when player come in contact with the parcel
                promptText.text = "Press 'E' to pick up parcel.";
                Debug.LogError("Prompt should appear!");
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

            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.None; // set display to none again when player walk away from the parcel
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

                Debug.Log($"Attempting to add parcel: ID={parcelData.parcelID}, Position={parcelData.position}, Sprite={parcelData.parcelSprite}, Assigned NPC: {parcelData.assignedNPC}");


                bool added = inventory.AddParcelToInventory(parcelData.parcelID, parcelData.parcelSprite, parcelData.position, parcelData.assignedNPC); // add to inventory
                inventory.parcelPickedUp++;

                if (added)
                {
                    Debug.Log("Parcel picked up!");

                    if (promptText != null)
                    {
                        promptText.style.display = DisplayStyle.Flex;  // if added to inventory, text is gone
                        promptText.text = "Parcel picked up and added to inventory!";
                    }
                    inventory.UpdateInventoryUI();

                    Invoke(nameof(destroyParcel), 1f);
                }
                else
                {
                    Debug.Log("Inventory full, cannot pick up the parcel.");
                    if (promptText != null)
                    {
                        promptText.style.display = DisplayStyle.Flex;  // if added to inventory, text is gone

                        promptText.text = "Inventory full, can't pick up the parcel!";
                    }
                }
            }
        }
    }

    private void destroyParcel()
    {
        gameObject.SetActive(false); // Hide the parcel
        Destroy(gameObject, 0.1f); // Destroy it after a short delay
    }

    // Change the position of the text prompt (Got error with code, need fixing)
    /*private void UpdatePromptPosition()
    {
        // Get the world position of the parcel (add some offset if needed)
        Vector3 parcelWorldPosition = transform.position + Vector3.up * 2f; // Adjust the height (2f) as needed

        // Convert world position to screen space
        Vector3 screenPos = Camera.main.WorldToScreenPoint(parcelWorldPosition);

        // Adjust Y position to be above the parcel
        screenPos.y += 50f; // Add an offset to push the text up (you can change this value)

        // Update the position of the UI text in screen space
        promptText.style.left = (screenPos.x);
        promptText.style.top = screenPos.y;
    }*/
}
