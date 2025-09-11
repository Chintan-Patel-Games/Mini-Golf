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
        public void GetDistanceToBall()
        {
            if (Camera.main == null) return;

            var plane = new Plane(Camera.main.transform.forward, BallController.Instance.transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float dist))
            {
                var clickWorldPos = ray.GetPoint(dist);
                model.ClickDistance = Vector3.Distance(clickWorldPos, BallController.Instance.transform.position);
            }
        }

        public void OnMouseDown(float clickDistance)
        {
            model.ClickDistance = clickDistance;
            model.CanRotate = true;
        }

        public void OnMouseUp() => model.CanRotate = false;

        public bool IsBallClick() => model.ClickDistance <= model.ClickDistanceLimit && model.BallInputEnabled;

        public bool IsCameraActive() => model.CameraInputEnabled;

        public void EnableBallInput(bool enabled) => model.EnableBallInput(enabled);

        public void EnableCameraInput(bool enabled) => model.EnableCameraInput(enabled);

        public bool IsInputEnabled() => model.BallInputEnabled && model.CameraInputEnabled;
    }
}