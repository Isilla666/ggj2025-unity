using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Events;
using Backend.Invoker;
using Backend.Messages;
using Backend.Registration;
using UnityEngine;

namespace InGameBehaviours
{
    public class BackendUserManager : MonoBehaviour
    {
        private ICustomMethodSignalListener _signalListener;
        private ISignalListener<ClientConnectedEvent.ClientData> _connectedListener;
        private ISignalListener<ClientConnectedEvent.ClientData> _disconnectedListener;
        private ISignalInvoke _signal;
        private readonly Dictionary<string, ClientConnectedEvent.ClientData> _connectedUsers = new(24);

        public IDictionary<string, string> GetUsersWithNames() =>
            _connectedUsers.ToDictionary(x => x.Key, v => v.Value.UserName);

        public event Action<string> OnUserConnectedEvent;
        public event Action<string> OnUserDisconnectedEvent;
        public event Action<string> OnUserStartShakeEvent;
        public event Action<string, int> OnUserStopEvent;
        public event Action<string> OnUserNoTimeStopEvent;

        private void OnEnable()
        {
            _signal = SignalRegistration<ISignalInvoke>.Resolve();
            _signalListener = SignalRegistration<ICustomMethodSignalListener>.Resolve();
            _connectedListener = SignalRegistration<ClientConnectedEvent>.Resolve();
            _disconnectedListener = SignalRegistration<ClientDisconnectedEvent>.Resolve();

            _connectedListener.OnValueChanged += OnClientConnected;
            _disconnectedListener.OnValueChanged += OnClientDisconnected;
            _signalListener.OnEvent += OnMethodInvoked;
        }

        private void OnDisable()
        {
            _connectedListener.OnValueChanged -= OnClientConnected;
            _disconnectedListener.OnValueChanged -= OnClientDisconnected;
            _signalListener.OnEvent -= OnMethodInvoked;
        }


        public void StateStart() =>
            _signal.SendToClients("start-music", null);

        public void StateStop() =>
            _signal.SendToClients("stop-music", null);

        public void BanUser(string userId, string reason) =>
            _signal.SendToClient(userId, "you-lose", reason);

        public void SendClientCharacter(string userId, string playerSit) =>
            _signal.SendToClient(userId, "you-info", playerSit);

        private void OnMethodInvoked(string method, CustomUserDataEvent.UserData data)
        {
            if (method == "start-shaking")
                OnUserStartShake(data);
            else if (method == "stop-shaking")
                OnUserStopShake(data);
        }

        private void OnClientConnected(ClientConnectedEvent.ClientData data)
        {
            /*
             * TODO
             *  По идее здесь мы должны создать персонажа для игрока, посадить персонажа и отправить данные о персонаже игроку в браузер
             */
            OnUserConnectedEvent?.Invoke(data.UserId);
            _connectedUsers[data.UserId] = data;
        }

        private void OnClientDisconnected(ClientConnectedEvent.ClientData data)
        {
            /*
             * TODO
             *  По идее здесь мы должны удалить персонажа игрока
             */
            OnUserDisconnectedEvent?.Invoke(data.UserId);
            _connectedUsers.Remove(data.UserId);
        }

        private void OnUserStartShake(CustomUserDataEvent.UserData data)
        {
            /*
             * TODO
             *  Метод, который вызывается, когда игрок трясет
             */
            OnUserStartShakeEvent?.Invoke(data.UserId);
        }

        private void OnUserStopShake(CustomUserDataEvent.UserData data)
        {
            /*
             * TODO
             *  Метод, который вызывается, когда игрок перестал трясти
             *  Если поступила команда, что музыка закончилась, тогда в data.UserCustomData будет еще значение в ms
             *      через сколько остановили тряску
             */

            if (!string.IsNullOrEmpty(data.UserCustomData.ToString()))
            {
                if (int.TryParse(data.UserCustomData.ToString(), out var time))
                {
                    // Если время есть - остановили после того, как музыка остановилась
                    OnUserStopEvent?.Invoke(data.UserId, time);
                }
            }
            else
            {
                // Если времени нет - остановили во время музыки
                OnUserNoTimeStopEvent?.Invoke(data.UserId);
            }
        }
    }
}