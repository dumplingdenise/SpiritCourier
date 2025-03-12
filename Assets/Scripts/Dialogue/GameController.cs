using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Puzzle, WaitingForDelivery }

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
    [SerializeField] PlayerController playerController;

    GameState state;

    // test code
    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/
    }

    private void OnEnable()
    {
        if (playerController == null)
        {
            playerController = Object.FindFirstObjectByType<PlayerController>();
        }
    }

    // test code
    public GameState GetCurrentState()
    {
        return state;
    }

    public void SetGameState(GameState newState)
    {
        state = newState;

        if (state == GameState.FreeRoam || state == GameState.WaitingForDelivery)
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
        Debug.Log($"Current state: {GetCurrentState()}");
        if (state == GameState.Puzzle)
        {
            Debug.LogError($"{state}");
            SetGameState(GameState.WaitingForDelivery);
            Debug.LogError($"{state}");
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
                SetGameState(GameState.WaitingForDelivery);
            }
        };
    }

    public void Update()
    {
        if (state == GameState.FreeRoam || state == GameState.WaitingForDelivery)
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
