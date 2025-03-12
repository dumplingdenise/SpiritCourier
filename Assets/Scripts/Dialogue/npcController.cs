using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class npcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] bool showButtonAfterDialog = false; // Toggle in Inspector
   // [SerializeField] string RotatePuzzle; // name of the puzzle scene

    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, showButtonAfterDialog));

    }
}
