using System;
using Backend.Registration;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Events
{
    public class UserDataEvent : JsonEvent<UserDataEvent.UserData>
    {
        protected override string MethodName => "image";

        private void Start() =>
            SignalRegistration<UserDataEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserDataEvent>.Unregister();

        public struct UserData
        {
            [JsonProperty("userId")] public string UserId;

            [JsonProperty("userName")] public string UserName;

            [JsonProperty("data")] public ImageData UserCustomData;
        }

        public struct ImageData
        {
            [JsonProperty("task")] public string task;

            [JsonProperty("image")] public string encryptedImage;

            public Texture2D GetImage()
            {
                if (string.IsNullOrEmpty(encryptedImage))
                    return new Texture2D(128, 128);

                string base64Data = encryptedImage.Substring(encryptedImage.IndexOf(",") + 1);
                byte[] imageData = Convert.FromBase64String(base64Data);
                Texture2D texture = new Texture2D(128, 128);
                if (texture.LoadImage(imageData))
                {
                    // Успешно загрузили текстуру
                    return texture;
                }
                else
                {
                    // Если загрузка не удалась
                    Debug.LogError("Не удалось загрузить текстуру из base64 строки.");
                    return new Texture2D(128, 128);
                }
            }
        }
    }
}