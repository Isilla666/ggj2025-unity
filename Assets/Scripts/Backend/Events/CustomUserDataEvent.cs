using System;
using Backend.Messages;
using Backend.Registration;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Events
{
    public sealed class CustomUserDataEvent : JsonEvent<CustomUserDataEvent.UserData>
    {
        [SerializeField] private string methodName;

        private readonly Lazy<ICustomMethodInvoke> _methodListenerLazy =
            new Lazy<ICustomMethodInvoke>(SignalRegistration<ICustomMethodInvoke>.Resolve);

        private ICustomMethodInvoke CustomMethodInvoke => _methodListenerLazy.Value;

        protected override string MethodName => methodName;

        protected override UserData ConvertToOutput(object input)
        {
            var output = base.ConvertToOutput(input);
            CustomMethodInvoke.Push(methodName, output);
            return output;
        }

        public struct UserData
        {
            [JsonProperty("userId")] public string UserId;

            [JsonProperty("userName")] public string UserName;

            [JsonProperty("data")] public object UserCustomData;
        }
    }
}