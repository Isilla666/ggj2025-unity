using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace ForTests.Examples
{
    public class SonyEricssonPlayer : MonoBehaviour
    {
        /// <summary>
        /// Сюда вставляй видео с музыкой и видео с паузой
        /// </summary>
        [System.Serializable]
        private class VideoData
        {
            public VideoClip activeClip;
            public VideoClip stopClip;
        }


        /// <summary>
        /// Видео плеер для музыки. Настроить его в Inspector надо.
        /// </summary>
        [SerializeField] private VideoPlayer activeSource;

        /// <summary>
        /// Видео плеер для "стоп"-видео.
        /// </summary>
        [SerializeField] private VideoPlayer stopSource;


        /// <summary>
        /// Данные видосов
        /// </summary>
        [SerializeField] private VideoData[] videoData;

        private int iterator = 0;
        private bool _nextVideoPrepared;
        private bool _isVideoPlaying;

        private Action _callback;

        [Button]
        public void NextClip(Action stopCallback)
        {
            _callback = stopCallback;
            var clip = videoData[iterator++ % videoData.Length];
            stopSource.clip = clip.stopClip;

            if (_isVideoPlaying)
            {
                activeSource.Stop();
                activeSource.loopPointReached -= OnVideoLoopReached;
                _nextVideoPrepared = false;
            }

            if (!_nextVideoPrepared)
            {
                activeSource.clip = clip.activeClip;
                activeSource.Prepare();
            }

            stopSource.Prepare();
            activeSource.Play();
            _nextVideoPrepared = false;
            _isVideoPlaying = true;
            activeSource.loopPointReached += OnVideoLoopReached;
        }

        private void OnVideoLoopReached(VideoPlayer source)
        {
            activeSource.loopPointReached -= OnVideoLoopReached;
            stopSource.Play();
            activeSource.Stop();

            var nextClip = videoData[(iterator + 1) % videoData.Length];
            activeSource.clip = nextClip.activeClip;
            activeSource.Prepare();
            _callback?.Invoke();
            _nextVideoPrepared = true;
            _isVideoPlaying = false;
            Debug.LogError("VIDEO END");
        }
    }
}