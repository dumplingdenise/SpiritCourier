using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelManager : MonoBehaviour
{
    private List<bool> parcelCollectedState; // Track the state of each parcel (collected or not)

    void Awake()
    {
        // Initialize the list based on the total number of parcels (6 in this case)
        parcelCollectedState = new List<bool>(new bool[6]);

        LoadParcelStates();
    }

    public void MarkParcelAsCollected(int parcelIndex)
    {
        if (parcelIndex >= 0 && parcelIndex < parcelCollectedState.Count)
        {
            parcelCollectedState[parcelIndex] = true;
            SaveParcelStates();
        }
    }

    public bool IsParcelCollected(int parcelIndex)
    {
        if (parcelIndex >= 0 && parcelIndex < parcelCollectedState.Count)
        {
            return parcelCollectedState[parcelIndex];
        }
        return false;
    }

    private void SaveParcelStates()
    {
        for (int i = 0; i < parcelCollectedState.Count; i++)
        {
            PlayerPrefs.SetInt("Parcel" + i, parcelCollectedState[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadParcelStates()
    {
        for (int i = 0; i < parcelCollectedState.Count; i++)
        {
            parcelCollectedState[i] = PlayerPrefs.GetInt("Parcel" + i, 0) == 1;
        }
    }

    // Optionally, reset the parcel states (e.g., for testing or new game)
    public void ResetParcelStates()
    {
        for (int i = 0; i < parcelCollectedState.Count; i++)
        {
            parcelCollectedState[i] = false;
        }
        SaveParcelStates();
    }
}
