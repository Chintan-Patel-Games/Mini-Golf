using UnityEngine;

namespace MiniGolf.Sound
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public Sounds[] audioList;
    }

    [System.Serializable]
    public struct Sounds
    {
        public SoundType soundType;
        public AudioClip audio;
    }
}