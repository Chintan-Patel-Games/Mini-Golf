using UnityEngine;

namespace MiniGolf.Level
{
    [CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        [Header("Level Settings")]
        public GameObject levelPrefab;
        public int par = 0; // 0 means no par assigned

        [Header("Animation Settings")]
        public float platformRiseDistance = 10f;
        public float platformRiseDuration = 1.2f;
        public float platformFallDuration = 0.8f;
    }
}