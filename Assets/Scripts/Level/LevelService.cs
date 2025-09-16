using MiniGolf.Main;
using System;
using UnityEngine;

namespace MiniGolf.Level
{
    public class LevelService
    {
        private readonly Transform parent;
        private readonly LevelSO[] levels;

        private int currentLevelIndex = 0;
        private LevelController currentLevel;

        public LevelService(LevelSO[] levels, Transform parent)
        {
            this.levels = levels;
            this.parent = parent;
        }

        public void StartLevel(Action onComplete)
        {
            // Destroy previous level if still around
            if (currentLevel != null)
                currentLevel.DestroyLevel();

            // Clear any leftover ball
            GameService.Instance.BallService.ClearBall();

            var model = levels[currentLevelIndex];
            currentLevel = new LevelController(model, parent);

            currentLevel.LevelRise(() =>
            {
                if (currentLevel.Model.par > 0)
                    GameService.Instance.UIService.SetPar(currentLevel.Model.par);

                GameService.Instance.UIService.SetStrokes(0);

                currentLevel.SpawnBall(() => onComplete?.Invoke());
            });
        }

        public void CompleteLevel(Action onComplete)
        {
            currentLevel.ClearBall();

            currentLevel.LevelFall(() =>
            {
                currentLevel.DestroyLevel();

                GameService.Instance.CameraManager.MoveVcamTo(() =>
                {
                    currentLevelIndex++;
                    if (currentLevelIndex >= levels.Length)
                        currentLevelIndex = 0;

                    StartLevel(onComplete);
                });
            });
        }

        // --- Exposed for external calls ---
        public void RiseLevel(Action onComplete = null) => currentLevel?.LevelRise(onComplete);
        public void FallLevel(Action onComplete = null) => currentLevel?.LevelFall(onComplete);
        public void DestroyLevel(Action onComplete = null) => currentLevel?.DestroyLevel();
    }
}