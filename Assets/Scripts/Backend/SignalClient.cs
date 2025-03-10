﻿using System;
using Backend.Invoker;
using Backend.Messages;
using Backend.Registration;
using Cysharp.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Backend
{
    // TODO https://habr.com/ru/articles/712918/
    public class SignalClient : MonoBehaviour
    {
        public string HubUrl;

        [SerializeField] private int maxPlayers;

        private void Awake() => DontDestroyOnLoad(gameObject);

        public SubscriberMono[] subscribers;

        private HubConnection _hubConnection;

        private ISignalInvoke _signalInvoke;

        public async UniTask<bool> Connect()
        {
            if (_hubConnection != null)
                await DisposeHub();

            _hubConnection = await CreateConnectionAsync();

            RegisterServices();

            bool connected = _hubConnection != null && _hubConnection.State == HubConnectionState.Connected;
            if (maxPlayers > 0 && connected)
                await _hubConnection.InvokeAsync("SetRoomPlayers", maxPlayers);

            return connected;
        }

        private async void OnApplicationQuit() =>
            await DisposeHub();

        private async void OnDestroy() =>
            await DisposeHub();


        private void RegisterServices()
        {
            SignalRegistration<ISignalInvoke>.Register(_signalInvoke);

            if (TryGetComponent<CustomMethodSignalListener>(out var custom) == false)
                custom = gameObject.AddComponent<CustomMethodSignalListener>();

            SignalRegistration<ICustomMethodSignalListener>.Register(custom);
            SignalRegistration<ICustomMethodInvoke>.Register(custom);
        }

        private async UniTask<HubConnection> CreateConnectionAsync()
        {
            //Создаем соединение с нашим написанным тестовым хабом
            var connection = new HubConnectionBuilder()
                .WithUrl(HubUrl)
                .WithAutomaticReconnect()
                .Build();

            Debug.Log("[Signal Client]: connection handle created");

            foreach (var subscriber in subscribers)
                subscriber.Subscribe(connection);

            while (connection.State != HubConnectionState.Connected)
            {
                try
                {
                    if (connection.State == HubConnectionState.Connecting)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(1));
                        continue;
                    }

                    Debug.Log("[Signal Client]: start connection");
                    await connection.StartAsync();
                    Debug.Log("[Signal Client]: connection finished");
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    await DisposeHub();
                    _signalInvoke = new NoConnectedInvoke();
                    return null;
                }
            }

            _signalInvoke = new SignalInvoke(connection);
            return connection;
        }

        private async UniTask DisposeHub()
        {
            foreach (var subscriber in subscribers)
                subscriber.Dispose();

            if (_hubConnection == null)
                return;

            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}