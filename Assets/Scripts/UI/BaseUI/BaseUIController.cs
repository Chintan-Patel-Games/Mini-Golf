namespace MiniGolf.UI.BaseUI
{
    /// <summary>
    /// Base class for all UI controllers in the game.
    /// Currently serves as a marker class for UI controllers.
    /// </summary>
    public class BaseUIController
    {
        protected BaseUIView view;

        public BaseUIController(BaseUIView viewToSet) => view = viewToSet;
        public void ShowUI() => view.ShowUI();
        public void HideUI() => view.HideUI();
    }
}