using MiniGolf.UI.GameplayUI;
using MiniGolf.UI.MainMenuUI;
using MiniGolf.UI.PauseUI;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGolf.UI
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private MainMenuUIView mainMenuUI;
        [SerializeField] private GameplayUIView gameplayUI;
        [SerializeField] private PauseUIView pauseUI;

        private MainMenuUIController mainMenuUIController;
        private GameplayUIController gameplayUIController;
        private PauseUIController pauseUIController;

        private void Awake()
        {
            mainMenuUIController = new MainMenuUIController(mainMenuUI);
            gameplayUIController = new GameplayUIController(gameplayUI);
            pauseUIController = new PauseUIController(pauseUI);
        }

        public void ShowUI(UIType uiType)
        {
            switch (uiType)
            {
                case UIType.MainMenu:
                    mainMenuUIController.ShowUI();
                    gameplayUIController.HideUI();
                    pauseUIController.HideUI();
                    break;
                
                case UIType.Gameplay:
                    gameplayUIController.ShowUI();
                    mainMenuUIController.HideUI();
                    pauseUIController.HideUI();
                    break;
                
                case UIType.Pause:
                    pauseUIController.ShowUI();
                    mainMenuUIController.HideUI();
                    break;
                
                default:
                    Debug.LogWarning("Unknown UIType: " + uiType);
                    break;
            }
        }

        public void SetPar(int par) => gameplayUIController.SetPar(par);
        public void SetStrokes(int strokes) => gameplayUIController.SetStrokes(strokes);
        public void SetPower(int normalizedPower) => gameplayUIController.SetPower(normalizedPower);
    }
}