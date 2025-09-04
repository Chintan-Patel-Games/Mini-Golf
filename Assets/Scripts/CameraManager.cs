using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float moveDuration = 1f;  // Duration for camera animations

    private float currentAngle = 0f;
    private Vector3 initialOffset;
    private CinemachineTransposer transposer;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    public CinemachineVirtualCamera cinemachineVirtualCamera => vcam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Save the position/rotation you set in Unity
        defaultPosition = vcam.transform.position;
        defaultRotation = vcam.transform.rotation;
    }

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();

        // Save the original offset (this includes height!)
        initialOffset = transposer.m_FollowOffset;
    }

    public void ResetCameraToDefault()
    {
        vcam.Follow = null;
        vcam.LookAt = null;

        vcam.transform.position = defaultPosition;
        vcam.transform.rotation = defaultRotation;
    }

    public void RotateCamera(float mouseX)
    {
        currentAngle += mouseX * rotationSpeed;

        // Rotate the original horizontal offset while preserving height
        Vector3 rotatedOffset = Quaternion.Euler(0, currentAngle, 0) * new Vector3(initialOffset.x, 0, initialOffset.z);
        rotatedOffset.y = initialOffset.y;

        transposer.m_FollowOffset = rotatedOffset;
    }

    public void SetTarget(Transform newTarget)
    {
        vcam.Follow = newTarget;
        vcam.LookAt = newTarget;
    }

    /// <summary>
    /// Animate the vcam transform to a target position and rotation
    /// </summary>
    public void MoveVcamTo(System.Action onComplete = null)
    {
        // Temporarily detach follow
        vcam.Follow = null;
        vcam.LookAt = null;

        vcam.transform.DOMove(defaultPosition, moveDuration).SetEase(Ease.InOutSine);
        vcam.transform.DORotateQuaternion(defaultRotation, moveDuration).SetEase(Ease.InOutSine)
            .OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// Instantly set vcam to target (ball or object)
    /// </summary>
    public void MoveVcamToInstant(Transform targetTransform)
    {
        vcam.transform.position = targetTransform.position;
        vcam.transform.rotation = Quaternion.LookRotation(targetTransform.position - vcam.transform.position);
    }

    public void SetCameraInstant(Vector3 position, Transform target)
    {
        if (cinemachineVirtualCamera != null)
        {
            cinemachineVirtualCamera.transform.position = position;
            cinemachineVirtualCamera.transform.LookAt(target); // optional, or startPoint
        }
    }

    public void PrepareForLevel(Transform startPoint)
    {
        if (cinemachineVirtualCamera == null) return;

        // Define your offset here (so it’s centralized)
        Vector3 camOffset = new Vector3(0, 3f, -8f);

        // Place the vcam at the offset relative to the start point
        cinemachineVirtualCamera.transform.position = startPoint.position + camOffset;
        cinemachineVirtualCamera.transform.LookAt(startPoint);
    }
}