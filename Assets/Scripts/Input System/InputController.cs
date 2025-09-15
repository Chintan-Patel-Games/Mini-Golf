using UnityEngine;

namespace MiniGolf.InputSystem
{
    public class InputController
    {
        private readonly InputModel model;

        public InputController(InputModel model) => this.model = model;

        /// <summary>
        /// Calculates distance between the mouse click position and the ball.
        /// </summary>
        public void GetDistanceToBall(Vector3? ballPosition)
        {
            if (Camera.main == null) return;

            var plane = new Plane(Camera.main.transform.forward, ballPosition.Value);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float dist))
            {
                var clickWorldPos = ray.GetPoint(dist);
                model.ClickDistance = Vector3.Distance(clickWorldPos, ballPosition.Value);
            }
        }

        public void StartDrag(float clickDistance)
        {
            model.ClickDistance = clickDistance;
            model.CanRotate = true;
        }

        public void EndDrag() => model.CanRotate = false;

        public bool IsBallClick() => model.ClickDistance <= model.ClickDistanceLimit && model.BallInputEnabled;

        public bool IsCameraActive() => model.CameraInputEnabled;

        public void EnableBallInput(bool enabled) => model.EnableBallInput(enabled);

        public void EnableCameraInput(bool enabled) => model.EnableCameraInput(enabled);

        public bool IsInputEnabled() => model.BallInputEnabled && model.CameraInputEnabled;
    }
}