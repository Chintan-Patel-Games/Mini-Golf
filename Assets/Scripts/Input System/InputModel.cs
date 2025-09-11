namespace MiniGolf.InputSystem
{
    [System.Serializable]
    public class InputModel
    {
        public float ClickDistanceLimit { get; set; } = 0.2f;
        public float ClickDistance { get; set; }
        public bool CanRotate { get; set; }

        public bool BallInputEnabled { get; private set; } = true;
        public bool CameraInputEnabled { get; private set; } = true;

        public void EnableBallInput(bool enabled) => BallInputEnabled = enabled;
        public void EnableCameraInput(bool enabled) => CameraInputEnabled = enabled;
    }
}