using DG.Tweening;
using UnityEngine;

public abstract class BaseUIController : MonoBehaviour, IUIController
{
    [SerializeField] protected CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 0.3f;

    public virtual void ShowUI()
    {
        canvasGroup.DOFade(1f, fadeDuration);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void HideUI()
    {
        canvasGroup.DOFade(0f, fadeDuration);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}