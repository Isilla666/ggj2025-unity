using Backend.Registration;
using UnityEngine;

namespace InGameBehaviours
{
    public abstract class MonoHubListener<TEvent, TData> : MonoBehaviour
        where TEvent : ISignalListener<TData>
    {
        protected TEvent Event;

        protected virtual void OnEnable()
        {
            Event = SignalRegistration<TEvent>.Resolve();

            if (Event != null)
                Event.OnValueChanged += OnValueChanged;
        }

        protected virtual void OnDisable()
        {
            if (Event != null)
                Event.OnValueChanged -= OnValueChanged;
        }

        protected abstract void OnValueChanged(TData data);
    }
}