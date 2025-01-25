using Backend.Events;
using Backend.Registration;

namespace ForTests.Events
{
    public class UserStopShakeEvent : JsonEvent<UserGetInfoEvent.UserConnectedData>
    {
        protected override string MethodName => "stop-shaking";

        private void Start() =>
            SignalRegistration<UserStopShakeEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserStopShakeEvent>.Unregister();
    }
}