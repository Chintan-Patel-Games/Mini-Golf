using UnityEngine;

namespace MiniGolf.Ball
{
    [CreateAssetMenu(fileName = "BallModel", menuName = "MiniGolf/Ball/BallModel")]
    public class BallSO : ScriptableObject
    {
        [Header("Physics Settings")]
        public float maxForce = 10f;
        public float dragSensitivity = 10f;

        [Header("References")]
        public LayerMask rayLayer;
    }
}