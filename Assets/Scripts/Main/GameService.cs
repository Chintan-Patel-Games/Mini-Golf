using MiniGolf.Ball;
using MiniGolf.Event;
using MiniGolf.GameState;
using MiniGolf.InputSystem;
using MiniGolf.Level;
using MiniGolf.Sound;
using MiniGolf.UI;
using MiniGolf.Utilities;
using UnityEngine;

namespace MiniGolf.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        // Services:
        public SoundService SoundService { get; private set; }
        public GameStateManager GameStateManager { get; private set; }
        public EventService EventService { get; private set; }
        public LevelService LevelService { get; private set; }
        public BallService BallService { get; private set; }
        public InputService InputService { get; private set; }

        [Header("Scene Services")]
        [SerializeField] private CameraManager cameraManager;
        public CameraManager CameraManager => cameraManager;

        [SerializeField] private UIService uiService;
        public UIService UIService => uiService;

        [SerializeField] private SoundSO soundSO;

        [Header("AudioSource References")]
        [SerializeField] private AudioSource SFXSource;
        [SerializeField] private AudioSource BGMSource;

        [Header("Level References")]
        [SerializeField] private LevelSO[] levelPrefabs;
        [SerializeField] private Transform levelParent;

        [Header("Golf Ball References")]
        [SerializeField] private GameObject ballPrefab;
        public GameObject BallPrefab => ballPrefab;

        [SerializeField] private BallSO ballModel;

        protected override void Awake()
        {
            base.Awake();
            EventService = new EventService();
            LevelService = new LevelService(levelPrefabs, levelParent);
            BallService = new BallService(ballModel);
            GameStateManager = new GameStateManager();
            InputService = new InputService();
            SoundService = new SoundService(soundSO, SFXSource, BGMSource);
        }

        private void Start() => GameStateManager.Initialize();

        private void Update()
        {
            InputService.ProcessInput(BallService.GetBallPosition());
            BallService.TickUpdate();
        }

        private void FixedUpdate() => BallService.TickFixedUpdate();

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