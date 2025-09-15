using UnityEngine;

namespace MiniGolf.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform startPosition;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject areaAffector;

        private Rigidbody rb;
        private BallController controller;

        public Rigidbody Rb => rb;
        public Transform StartPosition => startPosition;
        public LineRenderer LineRenderer => lineRenderer;
        public GameObject AreaAffector => areaAffector;

        private void Awake() => rb = GetComponent<Rigidbody>();
        private void OnTriggerEnter(Collider other) => controller?.OnTriggerEnter(other);
        public void SetController(BallController controllerToSet) => controller = controllerToSet;
    }
}