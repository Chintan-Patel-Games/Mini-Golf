using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGolf.UI.GameplayUI
{
    /// <summary>
    /// View for the Gameplay UI.
    /// Handles displaying score, strokes, and other in-game information.
    /// </summary>
    public class GameplayUIView : BaseUIView
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private TextMeshProUGUI parText;
        [SerializeField] private TextMeshProUGUI strokesText;
        [SerializeField] private Slider powerSlider;

        private GameplayUIController controller;

        public void SetController(GameplayUIController controllerToSet)
        {
            controller = controllerToSet;
            SubscribeToButtonClicks();
        }

        private void SubscribeToButtonClicks() => pauseButton.onClick.AddListener(controller.OnPauseButton);
        private void UnSubscribeFromButtonClicks() => pauseButton.onClick.RemoveListener(controller.OnPauseButton);
        public void SetPar(int par) => parText.text = $"Par {par}";
        public void SetStrokes(int strokes) => strokesText.text = $"Strokes {strokes}";
        public void SetPower(float normalizedPower) => powerSlider.value = normalizedPower;
        private void OnDestroy() => UnSubscribeFromButtonClicks();
    }
}