using Cysharp.Threading.Tasks;

namespace Backend.Invoker
{
    public interface ISignalInvoke
    {
        bool WithConnection { get; }

        UniTask SendCommandToChangeState(int state);

        UniTask SendToClients(string method, object data);

        UniTask SendToClient(string clientId, string method, object data);
    }
}