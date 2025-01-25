using Backend.Events;
using Backend.Registration;
using Newtonsoft.Json;
using UnityEngine;

namespace ForTests.Events
{
    public class UserGetInfoEvent : JsonEvent<UserGetInfoEvent.UserConnectedData>
    {
        protected override string MethodName => "get-info";

        private void Start() =>
            SignalRegistration<UserGetInfoEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserGetInfoEvent>.Unregister();

        protected override void ValueChanged(object arg)
        {
            base.ValueChanged(arg);
            Debug.Log($"User connected. GetInfo {Data.userId}, {Data.userName}");
        }

        public struct UserConnectedData
        {
            [JsonProperty("userId")] public string userId;
            [JsonProperty("userName")] public string userName;
            [JsonProperty("data")] public object data;
        }
    }
}