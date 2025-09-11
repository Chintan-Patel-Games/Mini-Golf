using UnityEngine;
using UnityEngine.UI;

namespace MiniGolf.UI.MainMenuUI
{
    /// <summary>
    /// View for the Main Menu UI.
    /// Handles button references and interactions.
    /// </summary>
    public class MainMenuUIView : BaseUIView
    {
        [Header("Gameplay UI Elements")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private MainMenuUIController controller;

        public void SetController(MainMenuUIController controllerToSet)
        {
            controller = controllerToSet;
            SubscribeToButtonClicks();
        }

        private void SubscribeToButtonClicks()
        {
            startButton.onClick.AddListener(controller.OnStartButton);
            quitButton.onClick.AddListener(controller.OnQuitButton);
        }

        private void UnSubscribeFromButtonClicks()
        {
            startButton.onClick.RemoveListener(controller.OnStartButton);
            quitButton.onClick.RemoveListener(controller.OnQuitButton);
        }

        private void OnDestroy() => UnSubscribeFromButtonClicks();
    }
}