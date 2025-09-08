using UnityEngine;
using UnityEngine.UI;

public class PauseUIController : BaseUIController
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button homeButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(OnRetryButton);
        homeButton.onClick.AddListener(OnHomeButton);
    }

    public void OnRetryButton() { GameStateManager.Instance.ChangeState(GameState.LevelSetup); }

    public void OnHomeButton() { GameStateManager.Instance.ChangeState(GameState.MainMenu); }
}