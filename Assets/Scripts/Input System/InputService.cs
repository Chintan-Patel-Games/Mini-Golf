using MiniGolf.Main;
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

        /// <summary>
        /// Handles mouse input for ball and camera.
        /// </summary>
        public void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0) && !model.CanRotate)
            {
                controller.GetDistanceToBall();
                controller.OnMouseDown(model.ClickDistance);

                if (controller.IsBallClick())
                    GameService.Instance.GetBallController().MouseDownMethod();
                //  GameService.Instance.EventService.OnMouseDown.InvokeEvent();
            }

            if (model.CanRotate)
            {
                if (Input.GetMouseButton(0))
                {
                    if (controller.IsBallClick())
                        GameService.Instance.GetBallController().MouseNormalMethod();
                    //  GameService.Instance.EventService.OnMouseNormal.InvokeEvent();
                    else if (controller.IsCameraActive())
                        GameService.Instance.CameraManager.RotateCamera(Input.GetAxis("Mouse X"));
                }

                if (Input.GetMouseButtonUp(0))
                {
                    controller.OnMouseUp();
                    if (controller.IsBallClick())
                        GameService.Instance.GetBallController().MouseUpMethod();
                    //  GameService.Instance.EventService.OnMouseUp.InvokeEvent();
                }
            }
        }

        /// <summary>
        /// Handles Escape key for toggling pause.
        /// </summary>
        public void HandlePauseInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameService.Instance.GameStateManager.CurrentState == GameState.Paused)
                    GameService.Instance.GameStateManager.ChangeState(GameState.PlayerInput); // Resume
                else
                    GameService.Instance.GameStateManager.ChangeState(GameState.Paused);      // Pause
                //  GameService.Instance.EventService.OnPause.InvokeEvent();
            }
        }

        public void EnableBallInput(bool enable) => controller.EnableBallInput(enable);

        public void EnableCameraInput(bool enable) => controller.EnableCameraInput(enable);

        public bool IsInputEnabled() => controller.IsInputEnabled();
    }
}