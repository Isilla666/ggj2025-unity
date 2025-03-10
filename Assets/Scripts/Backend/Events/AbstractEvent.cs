using System;
using Backend.Registration;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Events
{
    public abstract class AbstractEvent<TInput, TOutput> : SubscriberMono,
        ISignalListener<TOutput>
    {
        public TOutput Data { get; private set; }

        public UnityEvent<TOutput> onValueChanged;

        protected abstract string MethodName { get; }

        public override void Subscribe(HubConnection connection)
        {
            if (string.IsNullOrEmpty(MethodName))
            {
                Debug.LogError($"For {gameObject.name} method name is does not exists.");
                return;
            }

            DisposableListener = connection.On<TInput>(MethodName, ValueChanged);
        }

        protected virtual void ValueChanged(TInput arg)
        {
            Debug.Log($"[ON EVENT] {MethodName} value is {arg}");
            Data = ConvertToOutput(arg);
            onValueChanged?.Invoke(Data);
        }

        protected abstract TOutput ConvertToOutput(TInput input);

        public event UnityAction<TOutput> OnValueChanged
        {
            add => onValueChanged.AddListener(value);

            remove => onValueChanged.RemoveListener(value);
        }
    }
}