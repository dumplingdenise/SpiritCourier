using UnityEngine;
using UnityEngine.UI;


public class FindingWaldo : MonoBehaviour
{
    public GameObject ObjectFoundUI; // UI panel that appears when waldo is found


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure the button is assigned in the inspector
        GetComponent<Button>().onClick.AddListener(FoundWaldo);

    }

    void FoundWaldo()
    {
        Debug.Log("You Found It!");
        ObjectFoundUI.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
