using MiniGolf.Event;
using MiniGolf.InputSystem;
using MiniGolf.UI;
using MiniGolf.Utilities;
using UnityEngine;

namespace MiniGolf.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        // Services:
        //public SoundService SoundService { get; private set; }
        public GameStateManager GameStateManager { get; private set; }
        public EventService EventService { get; private set; }
        public BallController BallController { get; private set; }
        public InputService InputService { get; private set; }

        // Scene services
        [SerializeField] private CameraManager cameraManager;
        public CameraManager CameraManager => cameraManager;

        [SerializeField] private LevelManager levelManager;
        public LevelManager LevelManager => levelManager;

        [SerializeField] private UIService uiService;
        public UIService UIService => uiService;

        //[SerializeField] private SoundSO soundSO;

        //[Header("AudioSource References")]
        //[SerializeField] private AudioSource sfxSource;
        //[SerializeField] private AudioSource bgMusicSource;

        protected override void Awake()
        {
            base.Awake();
            EventService = new EventService();
            GameStateManager = new GameStateManager();
            InputService = new InputService();
            //SoundService = new SoundService(soundSO, sfxSource, bgMusicSource);
        }

        private void Start() => GameStateManager.Initialize();

        private void Update()
        {
            if (InputService.IsInputEnabled())
            {
                InputService.HandleMouseInput();
                InputService.HandlePauseInput();
            }
        }

        public BallController GetBallController() => levelManager.BallController;

        public void OnExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            UIService.ShowMessagePopupUI(StringConstants.WebGLCloseGamePopup);
#else
            Application.Quit();
#endif
        }
    }
}