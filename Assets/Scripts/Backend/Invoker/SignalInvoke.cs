using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Backend.Invoker
{
    internal sealed class SignalInvoke : ISignalInvoke
    {
        private readonly HubConnection _connection;

        public SignalInvoke(HubConnection connection) =>
            _connection = connection;


        public bool WithConnection => _connection.State == HubConnectionState.Connected;

        public async UniTask SendCommandToChangeState(int state)
        {
            if (_connection.State != HubConnectionState.Connected)
                return;

            await SendMethod("gameState", state);
        }

        private async UniTask SendMethod(string method, object data)
        {
            var payload = new JsonPayload(method, data);
            await _connection.SendAsync("SendToClients", JsonConvert.SerializeObject(payload));
        }

        [Serializable]
        public class JsonPayload
        {
            public string method;
            public object data;

            public JsonPayload(string method, object data)
            {
                this.method = method;
                this.data = data;
            }
        }
    }
}