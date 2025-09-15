using MiniGolf.UI.BaseUI;
using MiniGolf.Main;

namespace MiniGolf.UI.PauseUI
{
    /// <summary>
    /// Controller for the Pause UI.
    /// Handles button interactions and communicates with the GameStateManager.
    /// </summary>
    public class PauseUIController : BaseUIController
    {
        public PauseUIController(PauseUIView view) : base(view)
        {
            view.SetController(this);
            HideUI();
        }

        public void OnRetryButton() => GameService.Instance.GameStateManager.ChangeState(GameState.LevelSetup);
        public void OnHomeButton() => GameService.Instance.GameStateManager.ChangeState(GameState.MainMenu);
    }
}