using MiniGolf.UI.BaseUI;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGolf.UI.PauseUI
{
    /// <summary>
    /// View for the Pause UI.
    /// Handles button references and interactions.
    /// </summary>
    public class PauseUIView : BaseUIView
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private Button homeButton;

        private PauseUIController controller;

        public void SetController(PauseUIController controllerToSet)
        {
            controller = controllerToSet;
            SubscribeToButtonClicks();
        }

        private void SubscribeToButtonClicks()
        {
            retryButton.onClick.AddListener(controller.OnRetryButton);
            homeButton.onClick.AddListener(controller.OnHomeButton);
        }

        private void UnSubscribeFromButtonClicks()
        {
            retryButton.onClick.RemoveListener(controller.OnRetryButton);
            homeButton.onClick.RemoveListener(controller.OnHomeButton);
        }

        private void OnDestroy() => UnSubscribeFromButtonClicks();
    }
}