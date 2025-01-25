using Backend.Registration;

namespace Backend.Events
{
    public class ClientDisconnectedEvent : JsonEvent<ClientConnectedEvent.ClientData>
    {
        protected override string MethodName => "client-disconnected";

        private void Start() =>
            SignalRegistration<ClientDisconnectedEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<ClientDisconnectedEvent>.Unregister();
    }
}