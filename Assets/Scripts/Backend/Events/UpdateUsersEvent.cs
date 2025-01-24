using Backend.Registration;

namespace Backend.Events
{
    public class UpdateUsersEvent : JsonEvent<UpdateUsersEvent.UsersCount>
    {
        protected override string MethodName => "clients-count";

        private void Start() =>
            SignalRegistration<UpdateUsersEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UpdateUsersEvent>.Unregister();

        public struct UsersCount
        {
            public int Count;
        }
    }
}