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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Save the position/rotation you set in Unity
        defaultPosition = vcam.transform.position;
        defaultRotation = vcam.transform.rotation;
    }

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        initialOffset = transposer.m_FollowOffset;  // Save the original offset

        // Always follow the singleton ball
        if (BallController.Instance != null)
            SetTarget(BallController.Instance.transform);
    }

    public void RotateCamera(float mouseX)
    {
        currentAngle += mouseX * rotationSpeed;

        Vector3 horiz = new Vector3(initialOffset.x, 0, initialOffset.z);
        Vector3 rotated = Quaternion.Euler(0, currentAngle, 0) * horiz;
        rotated.y = initialOffset.y;

        transposer.m_FollowOffset = rotated;
    }

    public void SetTarget(Transform newTarget, bool preserveWorldPosition = true, bool tweenToDefaultOffset = true)
    {
        if (newTarget == null) { vcam.Follow = null; vcam.LookAt = null; return; }
        if (transposer == null) transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();

        Vector3 camPos = vcam.transform.position;
        Vector3 tgtPos = newTarget.position;

        if (preserveWorldPosition)
        {
            Vector3 offset;
            switch (transposer.m_BindingMode)
            {
                case CinemachineTransposer.BindingMode.LockToTarget:
                case CinemachineTransposer.BindingMode.LockToTargetNoRoll:
                case CinemachineTransposer.BindingMode.LockToTargetWithWorldUp:
                    offset = Quaternion.Inverse(newTarget.rotation) * (camPos - tgtPos);
                    break;
                default:
                    offset = camPos - tgtPos;
                    break;
            }

            transposer.m_FollowOffset = offset;
        }

        vcam.Follow = newTarget;
        vcam.LookAt = newTarget;
        vcam.PreviousStateIsValid = false;

        var brain = Cinemachine.CinemachineCore.Instance.GetActiveBrain(0);
        if (brain != null) brain.ManualUpdate();

        initialOffset = transposer.m_FollowOffset;
        currentAngle = 0f;  // reset the input rotation accumulator

        if (tweenToDefaultOffset)
        {
            DOTween.Kill("CM_OffsetTween");
            DOTween.To(
                () => transposer.m_FollowOffset,
                v => transposer.m_FollowOffset = v,
                initialOffset, // now it's fresh baseline, not old one
                moveDuration
            ).SetEase(Ease.InOutSine).SetId("CM_OffsetTween");
        }
    }

    /// <summary>
    /// Animate the vcam transform to a target position and rotation
    /// </summary>
    public void MoveVcamTo(System.Action onComplete = null)
    {
        vcam.transform.DOMove(defaultPosition, moveDuration).SetEase(Ease.InOutSine);
        vcam.transform.DORotateQuaternion(defaultRotation, moveDuration).SetEase(Ease.InOutSine)
            .OnComplete(() => onComplete?.Invoke());
    }
}