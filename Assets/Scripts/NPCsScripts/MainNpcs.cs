using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNpcs : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcObjects; // Reference to the NPC Objects in the scene
    private List<NPCData> npcList = new List<NPCData>(); // storing their info into our own List. 

    [System.Serializable]
    public class NPCData
    {
        [SerializeField] public int npcID;
        public Vector2 npcPos;
        public GameObject npcInstance; // We store the instance here instead of the prefab
        /*public string difficulty;*/

        public NPCData(int npcID, Vector2 npcPos, GameObject npcInstance/*, string difficulty*/)
        {
            this.npcID = npcID;
            this.npcPos = npcPos;
            this.npcInstance = npcInstance;
            /*this.difficulty = difficulty;*/
        }
    }

    public List<NPCData> GetNPCList()
    {
        return npcList;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        InitializeNpcs();
        /*SpawnNpcs();*/
        LogNPCs(); // test logging of NPCs
    }

    void InitializeNpcs()
    {
        // Instead of instantiating new NPCs, retrieve existing NPCs from the scene.
        // Store their positions, and assign them an ID based on their index in the list.
        /*        for (int i = 0; i < npcObjects.Count; i++)
                {
                    GameObject npc = npcObjects[i];
                    Vector2 npcPosition = npc.transform.position; // Get position from scene

                    npcList.Add(new NPCData(i, npcPosition, npc));
                }*/

        for (int i = 0; i < npcObjects.Count; i++)
        {
            GameObject npc = npcObjects[i];

            if (npc == null)
            {
                Debug.LogError("NPC Object is null at index: " + i);
                continue;
            }

            Vector2 npcPosition = npc.transform.position;
            npcList.Add(new NPCData(i, npcPosition, npc));
            Debug.Log($"Added NPC ID: {i}, Name: {npc.name}");
        }
    }

    /*void SpawnNpcs()
    {
        // Since NPCs are already spawned in InitializeNpcs, this function may not be needed anymore.
        // You can use this function for future logic if needed (like moving NPCs or updating properties).
    }*/

    void LogNPCs() // test logging of NPCs
    {
        foreach (NPCData npc in npcList)
        {
            Debug.Log($"NPC ID: {npc.npcID}, Position: {npc.npcPos}, NPC Instance: {npc.npcInstance.name}");
        }
        Debug.Log($"NPC Count: {npcList.Count}");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
