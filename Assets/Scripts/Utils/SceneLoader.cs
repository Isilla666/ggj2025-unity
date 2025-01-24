using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

        public void Close()
        {
#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}