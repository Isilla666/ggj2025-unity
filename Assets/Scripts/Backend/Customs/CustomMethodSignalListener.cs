using System;
using System.Collections.Generic;
using Backend.Events;
using UnityEngine;

namespace Backend.Messages
{
    public class CustomMethodSignalListener : MonoBehaviour, ICustomMethodSignalListener, ICustomMethodInvoke
    {
        private struct QueuedData
        {
            public string method;
            public CustomUserDataEvent.UserData userData;

            public QueuedData(string method, CustomUserDataEvent.UserData userData)
            {
                this.method = method;
                this.userData = userData;
            }
        }

        private readonly Queue<QueuedData> _queue = new Queue<QueuedData>();

        public event Action<string, CustomUserDataEvent.UserData> OnEvent;

        public void Push(string method, CustomUserDataEvent.UserData userData)
        {
            Debug.Log($"[CUSTOM EVENT]: {method}");
            _queue.Enqueue(new QueuedData(method, userData));
        }

        private void Update()
        {
            while (_queue.Count > 0)
            {
                var data = _queue.Dequeue();
                OnEvent?.Invoke(data.method, data.userData);
            }
        }
    }
}