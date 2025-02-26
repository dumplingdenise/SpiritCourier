using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcels_Test : MonoBehaviour
{
    public GameObject[] parcel; // an array of multiple GameObject references allowing to assign different parcel prefabs in the inspector.
    public Vector2 areaSize = new Vector2(10f, 5f); // size of spawning area
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 5f;
    public float minDistBetweenParcels = 5f;

    private List<ParcelData> spawnedParcels = new List<ParcelData>(); // Creates a list that stores ParcelData objects
    private List<MainNpcs.NPCData> npcList; // declares a private list to store NPC data

    public class ParcelData
    {
        public Vector2 position;
        public int parcelID;
        /*public GameObject parcelPrefab; // if use GameObject is to store the full GameObject*/
        public Sprite parcelSprite; // to store the sprite only (image)
        public MainNpcs.NPCData assignedNPC;

        public ParcelData(Vector2 position, int parcelID, Sprite parcelPrefabSprite, MainNpcs.NPCData assignedNPC)
        {
            this.position = position;
            this.parcelID = parcelID;
            this.parcelSprite = parcelPrefabSprite;
            this.assignedNPC = assignedNPC;
        }
    }


    [System.Serializable] // Make this class show up in the inspector
    public class OneRectangle
    { // this class ask for 2 Vector2 
        public Vector2 rectBottomLeft;
        public Vector2 rectTopRight;
    }
    [SerializeField]
    public List<OneRectangle> noParcelsAreas = new List<OneRectangle>(); // Defining a list of oneRectangle
                                                                         // Every rectangle helps to prevent parcel from spawning inside that area

    void Start()
    {
        npcList = FindAnyObjectByType<MainNpcs>().GetNPCList(); // locates the MainNpcs script then using the GetNPCList function to retrieve the list of NPCs that were initialized in MainNpcs script.
        StartCoroutine(SpawnParcels());
    }

    IEnumerator SpawnParcels()
    {
        int spawnCount = 0;
        int parcelID = 0;
        while (spawnCount < 3)
        {
            /*foreach (MainNpcs.NPCData npc in npcList)
            {*/
            GameObject selectedParcel = parcel[Random.Range(0, parcel.Length)]; // Choose a random GameObject (the parcel prefab in inspector) and assign to selectedParcel used for spawning.

            Vector2 spawnPos = new Vector2(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            ) + (Vector2)transform.position; // spawn the parcel at random position on the x & y values.

            if (!IsPositionTooClose(spawnPos, selectedParcel) && !IsSpawningInExcludedArea(spawnPos))
            {
                MainNpcs.NPCData assignedNPC = npcList[Random.Range(0, npcList.Count)]; // assign a random NPC ID from the retried npcList to the parcel. 

                GameObject spawnedParcel = Instantiate(selectedParcel, spawnPos, Quaternion.identity, this.transform); // basically spawn the parcel with instantiate. random prefab, random generated position.


                Sprite parcelSprite = spawnedParcel.GetComponent<SpriteRenderer>().sprite; // get the Sprite from teh spawned parcel

                /*PickUpParcel pickUpParcel = spawnedParcel.GetComponent<PickUpParcel>(); // the next 3 lines: is to instantiate parcelData object that contains all the parcel info and assign it to the PickUpParcel script.
                if (pickUpParcel != null)
                {
                    pickUpParcel.parcelData = new Parcels_Test.ParcelData(spawnPos, parcelID, parcelSprite, assignedNPC);
                }*/

                spawnedParcels.Add(new ParcelData(spawnPos, parcelID, parcelSprite, assignedNPC)); // adding the parcel info in the private List created aboved

                Debug.Log($"Parcel ID: {parcelID}, Sprite: {parcelSprite} spawned at {spawnPos} is linked to NPC ID: {assignedNPC.npcID}");
                parcelID++;
                spawnCount++;
            }
            /*}*/
            /*yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));*/  // spawn parcel with delays
            yield return null; // no delay
        }
    }

    bool IsPositionTooClose(Vector2 posOfSpawnedParcel, GameObject ParcelObj)
    {
        float parcelRadius = GetParcelSize(ParcelObj);
        foreach (ParcelData spawnedParcel in spawnedParcels)
        {
            if (Vector2.Distance(spawnedParcel.position, posOfSpawnedParcel) < parcelRadius + minDistBetweenParcels)
            {
                return true;
            }
        }

        return false;
    }

    bool IsSpawningInExcludedArea(Vector2 posOfSpawnParcel)
    {
        foreach (OneRectangle rectangle in noParcelsAreas)
        {
            if (rectangle.rectBottomLeft.x < posOfSpawnParcel.x && posOfSpawnParcel.x < rectangle.rectTopRight.x)
            {
                if (rectangle.rectBottomLeft.y < posOfSpawnParcel.y && posOfSpawnParcel.y < rectangle.rectTopRight.y)
                {
                    Debug.Log("Illegally spawn");
                    return true;
                }
            }
        }
        return false;
    }

    float GetParcelSize(GameObject Parcel)
    {
        Sprite parcelSprite = Parcel.GetComponent<SpriteRenderer>().sprite;
        float parcelWidth = parcelSprite.bounds.size.x;
        float parcelHeight = parcelSprite.bounds.size.y;

        return Mathf.Max(parcelWidth, parcelHeight) / 2f;
    }

    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
          PlayerMovement player = collision.GetComponent<PlayerMovement>();
          foreach (ParcelData spawnedParcel in spawnedParcels)
          {
              if (player != null)
              {
                  Debug.Log("Player entered trigger!");
                  player.parcelCollected++;
                  Destroy();
              }
          }
      }*/

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireCube draw the outline DrawCube draws a translucent cube

        foreach (OneRectangle rectangle in noParcelsAreas)
        {
            if (rectangle.rectBottomLeft.x < rectangle.rectTopRight.x && rectangle.rectBottomLeft.y < rectangle.rectTopRight.y)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Vector3 center = Vector3.Lerp(rectangle.rectTopRight, rectangle.rectBottomLeft, 0.5f);
            float length = rectangle.rectTopRight.x - rectangle.rectBottomLeft.x;
            float breadth = rectangle.rectTopRight.y - rectangle.rectBottomLeft.y;

            Gizmos.DrawWireCube(center, new Vector3(length, breadth, 1f));
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, areaSize);

    }
}
