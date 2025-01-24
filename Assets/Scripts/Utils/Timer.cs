using System;
using UnityEngine;

namespace Utils
{
    public class Timer : MonoBehaviour
    {
        private int timeInSeconds;
        [SerializeField] private bool isPause;

        private float _currentTime;
        
        public bool IsStarted { get; private set; }

        public event Action<bool> OnPauseChange;
        public event Action OnStart;
        public event Action OnStop;

        public void StartTimer(float time)
        {
            timeInSeconds = (int) time;
            ResetTimer();
            IsStarted = true;
            
            OnStart?.Invoke();
        }

        public void SetPauseTimer(bool pause)
        {
            isPause = pause;
            
            OnPauseChange?.Invoke(isPause);
        }

        public void TogglePause() => SetPauseTimer(!isPause);

        public void StopTimer()
        {
            ResetTimer();
            IsStarted = false;
            
            OnStop?.Invoke();
        }

        public void ResetTimer()
        {
            SetPauseTimer(false);
            _currentTime = 0;
        }

        private void Update()
        {
            if (!IsStarted) return;
            
            _currentTime += Time.deltaTime;
            if(_currentTime > timeInSeconds)
                StopTimer();
        }
    }
}