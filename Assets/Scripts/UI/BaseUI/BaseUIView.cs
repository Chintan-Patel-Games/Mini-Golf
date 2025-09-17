using DG.Tweening;
using UnityEngine;

namespace MiniGolf.UI.BaseUI
{
    /// <summary>
    /// Base class for all UI views in the game.
    /// Handles showing and hiding the UI with fade animations.
    /// </summary>
    public abstract class BaseUIView : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.3f;

        public virtual void ShowUI()
        {
            gameObject.SetActive(true);
            canvasGroup.DOKill(); // stop any previous tweens
            canvasGroup.alpha = 0f; // ensure starts from 0
            canvasGroup.DOFade(1f, fadeDuration);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public virtual void HideUI()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            canvasGroup.DOKill(); // stop any previous tweens
            canvasGroup.DOFade(0f, fadeDuration)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}