using Backend.Events;

namespace Backend.Messages
{
    internal interface ICustomMethodInvoke
    {
        void Push(string method, CustomUserDataEvent.UserData userData);
    }
}