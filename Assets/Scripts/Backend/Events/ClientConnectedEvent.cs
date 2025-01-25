using Backend.Registration;
using Newtonsoft.Json;

namespace Backend.Events
{
    public class ClientConnectedEvent : JsonEvent<ClientConnectedEvent.ClientData>
    {
        public struct ClientData
        {
            [JsonProperty("id")] public string UserId;

            [JsonProperty("name")] public string UserName;
        }

        protected override string MethodName => "client-connected";

        private void Start() =>
            SignalRegistration<ClientConnectedEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<ClientConnectedEvent>.Unregister();
    }
}