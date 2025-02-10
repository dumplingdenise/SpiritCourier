using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleTouchs : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!PuzzleRotateControl.youWin)
            transform.Rotate(0f, 0f, 90f);
    }
}

