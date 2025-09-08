using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : BaseUIController
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButton);
        quitButton.onClick.AddListener(OnQuitButton);
    }

    public void OnStartButton() { GameStateManager.Instance.ChangeState(GameState.LevelSetup); }

    public void OnQuitButton() { Application.Quit(); }
}