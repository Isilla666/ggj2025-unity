using System;
using Cysharp.Threading.Tasks;
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

            await SendToClients("gameState", state);
        }

        public async UniTask SendToClients(string method, object data)
        {
            if (_connection.State != HubConnectionState.Connected)
                return;

            var jsonPayload = new JsonPayload(method, data);
            await _connection.SendAsync("SendToClients", JsonConvert.SerializeObject(jsonPayload));
        }

        public async UniTask SendToClient(string clientId, string method, object data)
        {
            if (_connection.State != HubConnectionState.Connected)
                return;

            var jsonPayload = new JsonPayload(method, data);
            await _connection.SendAsync("SendToClient", clientId, JsonConvert.SerializeObject(jsonPayload));
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