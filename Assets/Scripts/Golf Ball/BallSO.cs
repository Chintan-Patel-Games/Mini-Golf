using UnityEngine;

namespace MiniGolf.Ball
{
    [CreateAssetMenu(fileName = "BallSO", menuName = "ScriptableObjects/BallSO")]
    public class BallSO : ScriptableObject
    {
        [Header("Physics Settings")]
        public float maxForce = 10f;
        public float dragSensitivity = 10f;

        [Header("References")]
        public LayerMask rayLayer;
    }
}