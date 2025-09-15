using UnityEngine;

namespace MiniGolf.Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private Transform startPoint;
        public Transform StartPoint => startPoint;
    }
}