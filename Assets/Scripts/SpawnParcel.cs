using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnParcel : MonoBehaviour
{
    public GameObject[] Parcel;
    public Vector2 areaSize = new Vector2 (10f, 5f);
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 10f;
    public float minDistBetweenParcels = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private List<ParcelData> spawnedParcels = new List<ParcelData>();

    [System.Serializable] // Make this class show up in the inspector
    public class OneRectangle 
    { // this class ask for 2 Vector2 
        public Vector2 rectBottomLeft;
        public Vector2 rectTopRight;
    }
    [SerializeField]
    public List<OneRectangle> noParcelsAreas = new List<OneRectangle>(); // Definiing a list of oneRectangle
    // Every rectangle helps to prevent parcel from spawning inside that area
    
    void Start()
    {
        StartCoroutine(SpawnParcels());
    }

    IEnumerator SpawnParcels() 
    {
        int spawnCount = 0;
        while (spawnCount < 10) 
        {
            GameObject selectedParcel = Parcel[Random.Range(0, Parcel.Length)];
            
            Vector2 spawnPos = new Vector2(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            ) + (Vector2)transform.position;

            if (!IsPositionTooClose(spawnPos, selectedParcel) && !IsSpawningInExcludedArea(spawnPos)) 
            {
                Instantiate(selectedParcel, spawnPos, Quaternion.identity);
                spawnedParcels.Add(new ParcelData(spawnPos, selectedParcel));
                spawnCount++;
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
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

    float GetParcelSize(GameObject Parcel) { 
        Sprite parcelSprite = Parcel.GetComponent<SpriteRenderer>().sprite;
        float parcelWidth = parcelSprite.bounds.size.x;
        float parcelHeight = parcelSprite.bounds.size.y;

        return Mathf.Max(parcelWidth, parcelHeight) / 2f;
    }

    class ParcelData
    {
        public Vector2 position;
        public GameObject Parcel;

        public ParcelData(Vector2 position, GameObject Parcel)
        { 
            this.position = position;
            this.Parcel = Parcel;
        }
    }

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

    // Update is called once per frame
    /*  void Update()
      {

      }*/
}
