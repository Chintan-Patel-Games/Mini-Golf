using DG.Tweening;
using MiniGolf.Main;
using System;
using UnityEngine;

namespace MiniGolf.Level
{
    public class LevelController
    {
        private LevelSO model;
        private LevelView view;

        public LevelSO Model => model;

        public LevelController(LevelSO model, Transform parent)
        {
            this.model = model;
            var instance = UnityEngine.Object.Instantiate(model.levelPrefab, parent);
            view = instance.GetComponent<LevelView>();
        }

        #region Level Animations
        public void LevelRise(Action onComplete)
        {
            var originalPos = view.transform.position;
            view.transform.position -= Vector3.up * model.platformRiseDistance;

            view.transform.DOMoveY(originalPos.y, model.platformRiseDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void LevelFall(Action onComplete)
        {
            var targetPos = view.transform.position - Vector3.up * model.platformRiseDistance;

            view.transform.DOMoveY(targetPos.y, model.platformFallDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() => onComplete?.Invoke());
        }
        #endregion

        #region Ball Lifecycle
        public void SpawnBall(Action onComplete)
        {
            var start = view.StartPoint;

            // Delegate to BallService
            GameService.Instance.BallService.SpawnBall(start.position, start.rotation, ballController =>
            {
                // Camera follows ball
                GameService.Instance.CameraManager.SetTarget(ballController.View.transform, true, true);

                // Spawn animation
                var ballTransform = ballController.View.transform;
                ballTransform.localScale = Vector3.zero;
                ballTransform.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => onComplete?.Invoke());
            });
        }

        // Ball Cleanup
        public void ClearBall() => GameService.Instance.BallService.ClearBall();
        #endregion

        // Level Cleanup
        public void DestroyLevel()
        {
            if (view != null)
            {
                UnityEngine.Object.Destroy(view.gameObject);
                view = null;
            }
        }
    }
}