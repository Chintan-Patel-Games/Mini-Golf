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
            if (currentLevel != null)
                currentLevel.DestroyLevel();

            var model = levels[currentLevelIndex];
            currentLevel = new LevelController(model, parent);

            currentLevel.LevelRise(() =>
            {
                currentLevel.SpawnBall(() => onComplete?.Invoke());
            });
        }

        public void CompleteLevel(Action onComplete)
        {
            currentLevel.ClearBall();

            currentLevel.LevelFall(() =>
            {
                currentLevel.DestroyLevel();

                CameraManager.Instance.MoveVcamTo(() =>
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
        public void ResetLevel(Action onComplete = null) => StartLevel(onComplete);
    }
}