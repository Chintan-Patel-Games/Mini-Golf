using MiniGolf.Main;
using MiniGolf.UI;

public class GameStateManager
{
    public GameState CurrentState { get; private set; }

    public void Initialize() => ChangeState(GameState.MainMenu);

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                GameService.Instance.UIService.ShowUI(UIType.MainMenu);
                break;

            case GameState.LevelSetup:
                GameService.Instance.UIService.ShowUI(UIType.Gameplay);
                GameService.Instance.LevelService.StartLevel(() =>
                {
                    ChangeState(GameState.PlayerInput);
                });
                break;

            case GameState.PlayerInput:
                // Enable input
                GameService.Instance.InputService.EnableBallInput(true);
                GameService.Instance.InputService.EnableCameraInput(true);
                break;

            case GameState.BallMoving:
                // Disable input and start monitoring ball
                GameService.Instance.InputService.EnableBallInput(false);
                GameService.Instance.InputService.EnableCameraInput(true);
                break;

            case GameState.Paused:
                // Show pause UI and disable input
                GameService.Instance.InputService.EnableBallInput(false);
                GameService.Instance.InputService.EnableCameraInput(false);
                GameService.Instance.UIService.ShowUI(UIType.Pause);
                break;

            case GameState.LevelComplete:
                // Show UI or transition to next level
                GameService.Instance.InputService.EnableBallInput(false);
                GameService.Instance.InputService.EnableCameraInput(false);
                GameService.Instance.LevelService.CompleteLevel(() =>
                {
                    ChangeState(GameState.PlayerInput);
                });
                break;
        }
    }
}