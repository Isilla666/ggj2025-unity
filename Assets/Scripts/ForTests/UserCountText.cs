using TMPro;
using UnityEngine;

namespace ForTests
{
    public class UserCountText : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;

        public void UpdateUsers(int users) => tmpText.text = users.ToString();
    }
}