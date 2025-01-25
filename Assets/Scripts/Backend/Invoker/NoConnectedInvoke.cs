using Cysharp.Threading.Tasks;

namespace Backend.Invoker
{
    internal sealed class NoConnectedInvoke : ISignalInvoke
    {
        public bool WithConnection => false;

        public UniTask SendCommandToChangeState(int state) =>
            UniTask.CompletedTask;

        public UniTask SendToClients(string method, object data) =>
            UniTask.CompletedTask;

        public UniTask SendToClient(string clientId, string method, object data) =>
            UniTask.CompletedTask;
    }
}