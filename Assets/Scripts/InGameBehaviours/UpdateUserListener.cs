using Backend.Events;
using UnityEngine;
using UnityEngine.Events;

namespace InGameBehaviours
{
    public class UpdateUserListener : MonoHubListener<UpdateUsersEvent, UpdateUsersEvent.UsersCount>
    {
        [SerializeField] private UnityEvent<int> onValueChanged;
        public UnityEvent<int> OnUsersChanged => onValueChanged;

        private int _count;

        public int UserCount
        {
            get => _count;
            set
            {
                _count = value;
                onValueChanged?.Invoke(_count);
            }
        }


        protected override void OnEnable()
        {
            base.OnEnable();

            if (Event != null)
                OnValueChanged(Event.Data);
        }

        protected override void OnValueChanged(UpdateUsersEvent.UsersCount data) =>
            UserCount = data.Count;
    }
}