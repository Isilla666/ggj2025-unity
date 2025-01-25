using System;
using System.Collections.Generic;
using Backend.Events;
using Backend.Invoker;
using Backend.Messages;
using Backend.Registration;
using UnityEngine;

namespace ForTests.Examples
{
    public class UsersHolder : MonoBehaviour
    {
        public TestPlayer playerPrefab;
        public Transform playerParent;

        // TODO update with backend
        private ICustomMethodSignalListener _signalListener;
        private ISignalListener<ClientConnectedEvent.ClientData> _connectedListener;
        private ISignalListener<ClientConnectedEvent.ClientData> _disconnectedListener;

        private ISignalInvoke _signal;

        private readonly Dictionary<string, TestPlayer> _testPlayers = new(24);
        private readonly Dictionary<string, Action<CustomUserDataEvent.UserData>> _mappingMethods = new();

        private void Awake()
        {
            _mappingMethods["start-shaking"] = OnUserStartShake;
            _mappingMethods["stop-shaking"] = OnUserStopShake;
        }

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

        private void OnMethodInvoked(string arg1, CustomUserDataEvent.UserData arg2)
        {
            if (_mappingMethods.TryGetValue(arg1, out var act))
                act?.Invoke(arg2);
            else
                Debug.LogError($"No one method for {arg1}");
        }

        public void StateStart()
        {
            foreach (var player in _testPlayers.Values)
                player.ClearTimes();

            _signal.SendToClients("start-music", null);
        }

        public void StateStop()
        {
            _signal.SendToClients("stop-music", null);
        }


        private void OnClientConnected(ClientConnectedEvent.ClientData data)
        {
            /*
             * TODO
             *  По идее здесь мы должны создать персонажа для игрока, посадить персонажа и отправить данные о персонаже игроку в браузер
             */

            if (_testPlayers.TryGetValue(data.UserId, out var player))
                player.OnConnected(data.UserName);
            else
            {
                player = Instantiate(playerPrefab, playerParent, false);
                player.OnConnected(data.UserName);
                _testPlayers[data.UserId] = player;
                _signal.SendToClient(data.UserId, "gameState", 1);
            }
        }

        private void OnClientDisconnected(ClientConnectedEvent.ClientData data)
        {
            /*
             * TODO
             *  По идее здесь мы должны удалить персонажа игрока
             */


            if (_testPlayers.TryGetValue(data.UserId, out var player) == false)
                return;

            Destroy(player.gameObject);
            _testPlayers.Remove(data.UserId);
        }

        private void OnUserStartShake(CustomUserDataEvent.UserData data)
        {
            /*
             * TODO
             *  Метод, который вызывается, когда игрок трясет
             */

            if (_testPlayers.TryGetValue(data.UserId, out var player))
                player.OnShake();
        }

        private void OnUserStopShake(CustomUserDataEvent.UserData data)
        {
            /*
             * TODO
             *  Метод, который вызывается, когда игрок перестал трясти
             *  Если поступила команда, что музыка закончилась, тогда в data.UserCustomData будет еще значение в ms
             *      через сколько остановили тряску
             */


            if (_testPlayers.TryGetValue(data.UserId, out var player))
            {
                player.OnStopShake();

                var dataStr = data.UserCustomData.ToString();
                if (!string.IsNullOrEmpty(dataStr) && int.TryParse(dataStr, out var val))
                    player.OnStopShakeByTime(val);
            }
        }
    }
}