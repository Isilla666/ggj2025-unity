using Backend.Registration;

namespace Backend.Events
{
    public sealed class RoomCodeEvent : JsonEvent<RoomCodeEvent.Room>
    {
        protected override string MethodName => "room-code";

        private void Start() =>
            SignalRegistration<RoomCodeEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<RoomCodeEvent>.Unregister();

        public struct Room
        {
            public string Code;
        }
    }
}