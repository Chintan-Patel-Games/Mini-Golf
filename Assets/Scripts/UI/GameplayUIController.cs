using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : BaseUIController
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI parText;
    [SerializeField] private TextMeshProUGUI strokesText;
    [SerializeField] private Slider powerSlider;

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnPauseButton);
    }

    public void OnPauseButton() { GameStateManager.Instance.ChangeState(GameState.Paused); }
}
