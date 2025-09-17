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

    /// <summary>
    /// Reset current level (retry).
    /// Destroys ball, resets camera, plays fall animation, then reloads level.
    /// </summary>
    public void ResetLevel()
    {
        // Destroy ball if present
        GameService.Instance.BallService.ClearBall();

        // Play fall animation, then reload current level
        GameService.Instance.LevelService.FallLevel(() =>
        {
            // Reset camera
            GameService.Instance.CameraManager.MoveVcamTo(() =>
            {
                ChangeState(GameState.LevelSetup);
            });
        });
    }

    /// <summary>
    /// Go back to main menu.
    /// Clears everything, resets camera, shows Main Menu UI.
    /// </summary>
    public void Home()
    {
        // Destroy ball if present
        GameService.Instance.BallService.ClearBall();


        // Play fall animation, then reload current level
        GameService.Instance.LevelService.FallLevel(() =>
        {
            // Reset camera, then show main menu
            GameService.Instance.CameraManager.MoveVcamTo(() =>
            {
                GameService.Instance.LevelService.DestroyLevel();
                ChangeState(GameState.MainMenu);
            });
        });
    }
}