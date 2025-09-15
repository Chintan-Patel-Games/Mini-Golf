using MiniGolf.UI.BaseUI;
using MiniGolf.Main;
using UnityEngine;

namespace MiniGolf.UI.GameplayUI
{
    /// <summary>
    /// Controller for the Gameplay UI.
    /// Handles button interactions and UI logic.
    /// </summary>
    public class GameplayUIController : BaseUIController
    {
        public GameplayUIController(GameplayUIView view) : base(view)
        {
            view.SetController(this);
            HideUI();
        }

        public void OnPauseButton() => GameService.Instance.GameStateManager.ChangeState(GameState.Paused);
        public void SetPar(int par) => ((GameplayUIView)view).SetPar(par);
        public void SetStrokes(int strokes) => ((GameplayUIView)view).SetStrokes(strokes);
        public void SetPower(float normalizedPower) => ((GameplayUIView)view).SetPower(Mathf.Clamp01(normalizedPower / 100f));
    }
}