using System;
using Backend.Events;
using ForTests.Events;

namespace Backend.Messages
{
    public interface ICustomMethodSignalListener
    {
        event Action<string, CustomUserDataEvent.UserData> OnEvent;

    }
}