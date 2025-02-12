/*using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;
using static Parcels;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class DeliverParcel : MonoBehaviour
{
    private bool playerNearby = false;

    private Label promptText;

    private List<Inventory.inventoryParcelData> inventoryList;

    *//*public Button selectedParcel;*//*

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        *//* selectedParcel = Button.*//*

        inventoryList = FindAnyObjectByType<Inventory>().GetInventoryList();
        var uiDocument = GetComponentInParent<UIDocument>();
        if (uiDocument != null)
        {
            var rootVisualElement = uiDocument.rootVisualElement; // Get the UIDocument in the spawned parcel gameobject.

            *//*promptText = rootVisualElement.Q("DeliverLabel"); // get the Label*//*

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
                    promptText.text = "You have no parcels to deliver!";
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
        *//*var inventory = FindFirstObjectByType<Inventory>();
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && inventoryList.Count > 0 )
        {
            *//*parcelDeliver();*//*
            Debug.Log("Parcel delivered!");
            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.Flex;
                promptText.text = "Parcel delivered!";
            }

            // Call the RemoveParcelFromInventory method from the Inventory script
            inventory.RemoveParcelFromInventory();
        }*//*

        var inventory = FindFirstObjectByType<Inventory>();

        // Check if the player is nearby, and if they press "E" without selecting a parcel
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.selectedSlot < 0 || inventory.selectedSlot >= inventory.GetInventoryList().Count)
            {
                // If no parcel is selected
                if (promptText != null)
                {
                    promptText.style.display = DisplayStyle.Flex;
                    promptText.text = "Please select something!";
                }
                promptText.style.display = DisplayStyle.None;
            }
            else
            {
                // Parcel delivery logic
                Debug.Log("Parcel delivered!");
                if (promptText != null)
                {
                    promptText.style.display = DisplayStyle.Flex;
                    promptText.text = "Parcel successfully delivered!";
                    
                }

                // Call the RemoveParcelFromInventory method from the Inventory script
                inventory.RemoveParcelFromInventory();
            }
        }
    }

    *//* private void parcelDeliver()
     {
         if (inventoryList.Count > 0)
         {

         }
     }*//*
}
*/

using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;
using static Parcels;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class DeliverParcel : MonoBehaviour
{
    private bool playerNearby = false;

    private Label promptText;

    private List<Inventory.inventoryParcelData> inventoryList;

    /*public Button selectedParcel;*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* selectedParcel = Button.*/

        inventoryList = FindAnyObjectByType<Inventory>().GetInventoryList();
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
        /*var inventory = FindFirstObjectByType<Inventory>();
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && inventoryList.Count > 0 )
        {
            *//*parcelDeliver();*//*
            Debug.Log("Parcel delivered!");
            if (promptText != null)
            {
                promptText.style.display = DisplayStyle.Flex;
                promptText.text = "Parcel delivered!";
            }

            // Call the RemoveParcelFromInventory method from the Inventory script
            inventory.RemoveParcelFromInventory();
        }*/

        var inventory = FindFirstObjectByType<Inventory>();

        // Check if the player is nearby, and if they press "E" without selecting a parcel
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.selectedSlot < 0 || inventory.selectedSlot >= inventory.GetInventoryList().Count)
            {
                // If no parcel is selected
                if (promptText != null)
                {
                    promptText.style.display = DisplayStyle.Flex;
                    promptText.text = "Please select something!";
                }
            }
            else
            {
                // Parcel delivery logic
                Debug.Log("Parcel delivered!");
                if (promptText != null)
                {
                    promptText.text = ""; // resets previous message

                    promptText.style.display = DisplayStyle.Flex;
                    promptText.text = "Parcel successfully delivered to the spirit!";

                }

                // Call the RemoveParcelFromInventory method from the Inventory script
                inventory.RemoveParcelFromInventory();
            }
        }
    }

    /* private void parcelDeliver()
     {
         if (inventoryList.Count > 0)
         {

         }
     }*/
}
