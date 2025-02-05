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
    
    void Start()
    {
        StartCoroutine(SpawnParcels());
    }

    IEnumerator SpawnParcels() {
        int spawnCount = 0;
        while (spawnCount < 10) {
            GameObject selectedParcel = Parcel[Random.Range(0, Parcel.Length)];
            
            Vector2 spawnPos = new Vector2(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            ) + (Vector2)transform.position;

            if (!IsPositionTooClose(spawnPos, selectedParcel)) {
                Instantiate(selectedParcel, spawnPos, Quaternion.identity);
                spawnedParcels.Add(new ParcelData(spawnPos, selectedParcel));
                spawnCount++;
            }

            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
        }
    }

    bool IsPositionTooClose(Vector2 position, GameObject Parcel) {
        float parcelRadius = GetParcelSize(Parcel);
        foreach (ParcelData spawnedParcel in spawnedParcels) {
            if (Vector2.Distance(spawnedParcel.position, position) < parcelRadius + minDistBetweenParcels) {
                return true;
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

    class ParcelData {
        public Vector2 position;
        public GameObject Parcel;

        public ParcelData(Vector2 position, GameObject Parcel) { 
            this.position = position;
            this.Parcel = Parcel;
        }
    }

    // Update is called once per frame
  /*  void Update()
    {
        
    }*/
}
