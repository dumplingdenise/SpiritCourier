using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle, Puzzle }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    GameState state;

    // test code
    private void OnEnable()
    {
        if (playerController == null)
        {
            playerController = Object.FindFirstObjectByType<PlayerController>();
        }
    }

    public void SetGameState(GameState newState)
    {
        state = newState;

        if (state == GameState.FreeRoam)
        {
            if (playerController == null)
            {
                playerController = Object.FindFirstObjectByType<PlayerController>(); // Ensure it's found
            }
            playerController?.SetMoveWhenTalking(true); // Re-enable movement
        }
        else if (state == GameState.Puzzle)  // Disable movement when entering a puzzle
        {
            playerController?.SetMoveWhenTalking(false);
        }
    }

    private void Start()
    {
        // test code
        if (state == GameState.Puzzle)
        {
            Debug.LogError($"{state}");
            SetGameState(GameState.FreeRoam);
        }

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            playerController.SetMoveWhenTalking(false);
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                SetGameState(GameState.FreeRoam);
            }
        };
    }

    public void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (playerController == null)
            {
                playerController = Object.FindFirstObjectByType<PlayerController>(); // Reassign if lost
            }
            playerController?.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
