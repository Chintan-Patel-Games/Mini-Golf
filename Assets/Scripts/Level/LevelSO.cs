using UnityEngine;

namespace MiniGolf.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "MiniGolf/LevelData")]
    public class LevelSO : ScriptableObject
    {
        [Header("Level Settings")]
        public GameObject levelPrefab;

        [Header("Animation Settings")]
        public float platformRiseDistance = 10f;
        public float platformRiseDuration = 1.2f;
        public float platformFallDuration = 0.8f;
    }
}