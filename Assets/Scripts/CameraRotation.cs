using Cinemachine;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public static CameraRotation instance;

    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 5f;

    private float currentAngle = 0f;
    private Vector3 initialOffset;
    private CinemachineFramingTransposer transposer;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        // Save the original offset (this includes height!)
        initialOffset = transposer.m_TrackedObjectOffset;

        // Ensure vcam is set up to follow & look at the target
        vcam.Follow = target;
        vcam.LookAt = target;
    }

    public void RotateCamera(float mouseX)
    {
        currentAngle += mouseX * rotationSpeed;

        // Rotate the original horizontal offset while preserving height
        Vector3 rotatedOffset = Quaternion.Euler(0, currentAngle, 0) * new Vector3(initialOffset.x, 0, initialOffset.z);
        rotatedOffset.y = initialOffset.y;

        transposer.m_TrackedObjectOffset = rotatedOffset;
    }
}