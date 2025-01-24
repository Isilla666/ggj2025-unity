using Backend.Events;
using UnityEngine;
using UnityEngine.Events;

namespace InGameBehaviours
{
    public class RoomCodeListener : MonoHubListener<RoomCodeEvent, RoomCodeEvent.Room>
    {
        [SerializeField] private UnityEvent<string> onValueChanged;
        public UnityEvent<string> OnRoomCodeChanged => onValueChanged;

        public string RoomCode
        {
            get => _code;
            set
            {
                _code = value;
                onValueChanged?.Invoke(_code);
            }
        }

        private string _code;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Event != null)
                OnValueChanged(Event.Data);
        }

        protected override void OnValueChanged(RoomCodeEvent.Room data) =>
            RoomCode = data.Code;
    }
}