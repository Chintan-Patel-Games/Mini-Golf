using MiniGolf.UI.BaseUI;
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
        public void OnStartButton()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.UI_BUTTON_CLICK);
            GameService.Instance.GameStateManager.ChangeState(GameState.GameState.LevelSetup);
        }

        public void OnQuitButton()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.UI_BUTTON_CLICK);
            GameService.Instance.OnExitGame();
        }
    }
}