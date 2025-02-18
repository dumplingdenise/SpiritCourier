/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;


    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
} 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] bool showButtonAfterDialog = false; // Toggle in Inspector

    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, showButtonAfterDialog));
    }
}
