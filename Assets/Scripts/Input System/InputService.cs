using MiniGolf.Main;
using MiniGolf.UI;
using UnityEngine;

namespace MiniGolf.InputSystem
{
    public class InputService
    {
        private InputModel model;
        private InputController controller;

        public InputService()
        {
            model = new InputModel();
            controller = new InputController(model);
            EnableBallInput(false);
            EnableCameraInput(false);
        }

        public void ProcessInput(Vector3? ballPosition)
        {
            if (!IsInputEnabled()) return;
            if (ballPosition == null) return;

            HandleMouseInput(ballPosition);
            HandlePauseInput();
        }

        /// <summary>
        /// Handles mouse input for ball and camera.
        /// </summary>
        private void HandleMouseInput(Vector3? ballPosition)
        {
            if (ballPosition == null) return; // No balls in the scene

            if (Input.GetMouseButtonDown(0) && !model.CanRotate)
            {
                controller.GetDistanceToBall(ballPosition);
                controller.StartDrag(model.ClickDistance);

                if (controller.IsBallClick())
                    GameService.Instance.EventService.OnMouseDown.InvokeEvent();
            }

            if (model.CanRotate)
            {
                if (Input.GetMouseButton(0))
                {
                    if (controller.IsBallClick())
                        GameService.Instance.EventService.OnMouseNormal.InvokeEvent();
                    else if (controller.IsCameraActive())
                        GameService.Instance.CameraManager.RotateCamera(Input.GetAxis("Mouse X"));
                }

                if (Input.GetMouseButtonUp(0))
                {
                    controller.EndDrag();
                    if (controller.IsBallClick())
                        GameService.Instance.EventService.OnMouseUp.InvokeEvent();
                }
            }
        }

        /// <summary>
        /// Handles Escape key for toggling pause.
        /// </summary>
        private void HandlePauseInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameService.Instance.GameStateManager.CurrentState != GameState.Paused)
                {
                    GameService.Instance.GameStateManager.ChangeState(GameState.Paused);      // Pause
                    GameService.Instance.UIService.ShowUI(UIType.Pause);
                }
                else
                {
                    GameService.Instance.GameStateManager.ChangeState(GameState.PlayerInput); // Resume
                    GameService.Instance.UIService.ShowUI(UIType.Gameplay);
                }
            }
        }

        public void EnableBallInput(bool enable) => controller.EnableBallInput(enable);
        public void EnableCameraInput(bool enable) => controller.EnableCameraInput(enable);
        public bool IsInputEnabled() => controller.IsInputEnabled();
    }
}