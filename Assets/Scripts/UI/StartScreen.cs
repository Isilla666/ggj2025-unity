using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartScreen : UIScreen
    {
        [SerializeField] private Button playButton;
        private Action _callback;

        protected override void OnCompleteShow()
        {
            base.OnCompleteShow();
            playButton.interactable = true;
            playButton.onClick.AddListener(Hide);
        }

        protected override void OnCompleteHide()
        {
            base.OnCompleteHide();
            playButton.interactable = false;
            playButton.onClick.RemoveListener(Hide);
            _callback?.Invoke();
            _callback = null;
        }

        public void Show(Action startGameCallback)
        {
            if (IsVisible)
                return;

            _callback = startGameCallback;
            Show();
        }
    }
}