using MiniGolf.Main;

namespace MiniGolf.UI.MainMenuUI
{
    /// <summary>
    /// Controller for the Main Menu UI.
    /// Handles button interactions and UI logic.
    /// </summary>
    public class MainMenuUIController : BaseUIController
    {
        public MainMenuUIController(MainMenuUIView view) : base(view) => view.SetController(this);
        public void OnStartButton() => GameService.Instance.GameStateManager.ChangeState(GameState.LevelSetup);
        public void OnQuitButton() => GameService.Instance.OnExitGame();
    }
}