using Backend.Events;
using Backend.Registration;

namespace ForTests.Events
{
    public class UserStartShakeEvent : JsonEvent<UserGetInfoEvent.UserConnectedData>
    {
        protected override string MethodName => "start-shaking";

        private void Start() =>
            SignalRegistration<UserStartShakeEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserStartShakeEvent>.Unregister();
    }
}