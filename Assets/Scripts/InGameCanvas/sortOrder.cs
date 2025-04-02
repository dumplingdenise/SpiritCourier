using UnityEngine;

public class sortOrder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Canvas mapCanvas;
    public GameObject BigMap;

    public Canvas journalCanvas;
    public GameObject journal;

    public Canvas settingCanvas;
    public GameObject setting;
    void Start()
    {
        if (mapCanvas == null)
        {
            Debug.LogError("map Canvas not found");
            return;
        }
        if (BigMap == null)
        {
            Debug.LogError("big Map not found");
            return;
        }
        if (journalCanvas == null)
        {
            Debug.LogError("journal Canvas not found");
            return;
        }
        if (settingCanvas == null)
        {
            Debug.LogError("setting Canvas not found");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BigMap.activeSelf == true)
        {
            mapCanvas.GetComponent<Canvas>().sortingOrder = 2;
        }
        else 
        {
            mapCanvas.GetComponent<Canvas>().sortingOrder = 1;
        }

        if (setting.activeSelf == true)
        {
            settingCanvas.GetComponent<Canvas>().sortingOrder = 2;
        }
        else
        {
            settingCanvas.GetComponent<Canvas>().sortingOrder = 1;
        }

        if (journal.activeSelf == true)
        {
            journalCanvas.GetComponent<Canvas>().sortingOrder = 2;
        }
        else
        {
            journalCanvas.GetComponent<Canvas>().sortingOrder = 1;
        }
    }
}
