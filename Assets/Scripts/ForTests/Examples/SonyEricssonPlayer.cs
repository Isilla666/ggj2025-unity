using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace ForTests.Examples
{
    public class SonyEricssonPlayer : MonoBehaviour
    {
        /// <summary>
        /// Видео плеер для музыки. Настроить его в Inspector надо.
        /// </summary>
        [SerializeField] private VideoPlayer activeSource;

        /// <summary>
        /// Данные видосов
        /// </summary>
        [SerializeField] private VideoClip[] videoData;

        private int iterator = 0;
        private Action _callback;

        [Button]
        public void NextClip(Action stopCallback)
        {
            _callback = stopCallback;
            var clip = videoData[iterator++ % videoData.Length];
            activeSource.clip = clip;
            activeSource.Prepare();
            activeSource.Play();
            activeSource.loopPointReached += OnVideoLoopReached;
        }

        private void OnVideoLoopReached(VideoPlayer source)
        {
            activeSource.loopPointReached -= OnVideoLoopReached;
            activeSource.Stop();
            _callback?.Invoke();
        }
    }
}