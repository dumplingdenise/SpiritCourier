<<<<<<< HEAD
using System.Collections;
=======
/*using System.Collections;
>>>>>>> Denise's
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;


    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

<<<<<<< HEAD
} 
=======
} */

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
>>>>>>> Denise's
