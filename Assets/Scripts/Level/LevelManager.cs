using UnityEngine;
using DG.Tweening;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Prefabs")]
    [SerializeField] private GameObject[] levels;   // Prefabs of all levels
    [SerializeField] private Transform levelParent; // Where to spawn levels in hierarchy

    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab; // Ball prefab

    [Header("Animation Settings")]
    [SerializeField] private float platformRiseDistance = 10f;
    [SerializeField] private float platformRiseDuration = 1.2f;
    [SerializeField] private float platformFallDuration = 0.8f;

    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;
    private GameObject currentBallInstance;
    private Transform startPoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #region Level Setup
    public void StartLevelSetup(Action onComplete)
    {
        LoadLevel(currentLevelIndex);
        AnimateLevelRise(onComplete);  // Animate rise and spawn ball
    }

    private void LoadLevel(int index)
    {
        if (currentLevelInstance != null)  // Destroy old level if exists
            Destroy(currentLevelInstance);

        currentLevelInstance = Instantiate(levels[index], levelParent);
        UpdateStartPoint();

        // If ball already exists, destroy it for new level
        if (currentBallInstance != null)
            Destroy(currentBallInstance);
    }

    private void UpdateStartPoint()
    {
        startPoint = currentLevelInstance.transform.Find("StartPoint");
        if (startPoint == null)
            Debug.LogError("No StartPoint found in level prefab!");
    }
    #endregion

    #region Animations
    private void AnimateLevelRise(Action onComplete)
    {
        Vector3 originalPos = currentLevelInstance.transform.position;
        currentLevelInstance.transform.position -= Vector3.up * platformRiseDistance;

        currentLevelInstance.transform.DOMoveY(originalPos.y, platformRiseDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                SpawnBallWithPop(() => onComplete?.Invoke());
            });
    }

    private void SpawnBallWithPop(Action onComplete)
    {
        currentBallInstance = Instantiate(ballPrefab, startPoint.position, startPoint.rotation);
        currentBallInstance.transform.localScale = Vector3.zero;

        // Set vcam target immediately
        CameraManager.Instance.SetTarget(currentBallInstance.transform, preserveWorldPosition: true, tweenToDefaultOffset: true);

        currentBallInstance.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => onComplete?.Invoke() );
    }

    private void AnimateLevelFall(Action onComplete)
    {
        Vector3 targetPos = currentLevelInstance.transform.position - Vector3.up * platformRiseDistance;

        currentLevelInstance.transform.DOMoveY(targetPos.y, platformFallDuration)
            .SetEase(Ease.InCubic)
            .OnComplete(() => onComplete?.Invoke());
    }
    #endregion

    #region Level Complete
    public void LevelComplete(Action onComplete)
    {
        // Destroy current ball
        if (currentBallInstance != null)
        {
            Destroy(currentBallInstance);
            currentBallInstance = null;
        }

        AnimateLevelFall(() =>
        {
            CameraManager.Instance.MoveVcamTo(() =>
            {
                currentLevelIndex++;
                if (currentLevelIndex >= levels.Length) currentLevelIndex = 0;

                StartLevelSetup(onComplete);
            });
        });
    }
    #endregion

    public BallController BallController => currentBallInstance != null ? currentBallInstance.GetComponent<BallController>() : null;
}