using MiniGolf.Main;
using System;
using UnityEngine;

namespace MiniGolf.Ball
{
    public class BallService
    {
        private BallView ballView;
        private BallController controller;
        private readonly BallSO ballModel;

        public BallService(BallSO model) => ballModel = model;

        public void SpawnBall(Vector3 pos, Quaternion rot, Action<BallController> onComplete)
        {
            var instance = UnityEngine.Object.Instantiate(GameService.Instance.BallPrefab, pos, rot);
            ballView = instance.GetComponent<BallView>();

            controller = new BallController(ballView, ballModel); // constructor sets view
            controller.SubscribeToEvents();

            onComplete?.Invoke(controller);
        }

        public void ClearBall()
        {
            if (ballView != null)
            {
                if (controller != null)
                    controller.UnSubscribeToEvents();

                if (ballView != null)
                    UnityEngine.Object.Destroy(ballView.gameObject);

                ballView = null;
                controller = null;
            }
        }

        public bool HasBall() => ballView != null && controller != null;

        public void TickUpdate()
        {
            if (HasBall()) controller?.TickUpdate();
        }

        public void TickFixedUpdate()
        {
            if (HasBall()) controller?.TickFixedUpdate();
        }

        public Vector3? GetBallPosition() => ballView != null ? ballView.transform.position : (Vector3?)null;
    }
}