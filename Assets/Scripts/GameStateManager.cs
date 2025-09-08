using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.LevelSetup); // Kick off flow
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                //UIController.Instance.ShowMainMenuUI();
                break;

            case GameState.LevelSetup:
                // Setup/animate level and spawn ball
                LevelManager.Instance.StartLevelSetup(() =>
                {
                    ChangeState(GameState.PlayerInput);
                });
                break;

            case GameState.PlayerInput:
                // Enable input
                InputManager.Instance.EnableBallInput(true);
                InputManager.Instance.EnableCameraInput(true);
                break;

            case GameState.BallMoving:
                // Disable input and start monitoring ball
                InputManager.Instance.EnableBallInput(false);
                break;

            case GameState.LevelComplete:
                // Show UI or transition to next level
                //UIManager.Instance.ShowLevelComplete();
                InputManager.Instance.EnableBallInput(false);
                InputManager.Instance.EnableCameraInput(false);
                LevelManager.Instance.LevelComplete(() =>
                {
                    ChangeState(GameState.PlayerInput);
                });
                break;
        }
    }
}