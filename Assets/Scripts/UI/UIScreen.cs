using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private DOTweenAnimation animation;

        public bool IsVisible { get; private set; }

        public void Show()
        {
            IsVisible = true;
            animation.hasOnComplete = true;
            animation.onComplete.AddListener(OnCompleteShow);
            animation.DOPlayForward();
        }

        public void Hide()
        {
            IsVisible = false;
            animation.hasOnRewind = true;
            animation.onRewind.AddListener(OnCompleteHide);
            animation.DOPlayBackwards();
        }

        protected virtual void OnCompleteShow()
        {
            animation.onComplete.RemoveListener(OnCompleteShow);
            canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
        }

        protected virtual void OnCompleteHide()
        {
            animation.onRewind.RemoveListener(OnCompleteHide);
            canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
        }
    }
}