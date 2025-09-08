using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] private MainMenuUIController mainMenuUI;
    [SerializeField] private GameplayUIController gameplayUI;
    [SerializeField] private PauseUIController pauseUI;

    private IUIController[] allUIs;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        allUIs = new IUIController[] { mainMenuUI, gameplayUI, pauseUI };
    }

    //private void OnEnable()
    //{
    //    GameStateManager.ChangeState += HandleStateChanged;
    //}

    //private void OnDisable()
    //{
    //    GameStateManager.ChangeState -= HandleStateChanged;
    //}

    private void HandleStateChanged(GameState state)
    {
        foreach (var ui in allUIs)
            ui.HideUI();

        switch (state)
        {
            case GameState.MainMenu:
                mainMenuUI.ShowUI();
                break;

            case GameState.BallMoving:
                gameplayUI.ShowUI();
                break;

            case GameState.Paused:
                pauseUI.ShowUI();
                break;
        }
    }
}