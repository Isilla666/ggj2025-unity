using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ForTests.Examples
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text msText;
        [SerializeField] private Image image;

        public void OnConnected(string playerName)
        {
            playerNameText.text = playerName;
            msText.text = string.Empty;
        }

        public void ClearTimes() =>
            msText.text = string.Empty;

        public void OnStopShakeByTime(int time) =>
            msText.text = $"{time} ms";

        public void OnShake() =>
            image.color = Color.green;

        public void OnStopShake() =>
            image.color = Color.red;
    }
}