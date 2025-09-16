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

        public bool IsActive() => view.gameObject.activeSelf;
        
        public void OnResetButton()
        {
            view.HideUI();
            GameService.Instance.InputService.EnableBallInput(true);
            GameService.Instance.InputService.EnableCameraInput(true);
            GameService.Instance.GameStateManager.ResetLevel();
        }

        public void OnHomeButton()
        {
            view.HideUI();
            GameService.Instance.InputService.EnableBallInput(false);
            GameService.Instance.InputService.EnableCameraInput(false);
            GameService.Instance.GameStateManager.Home();
        }
    }
}