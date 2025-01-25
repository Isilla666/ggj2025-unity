using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ForTests.Examples
{
    public class MusicBeh : MonoBehaviour
    {
        public AudioSource audioSource;
        public UsersHolder usersHolder;

        private Coroutine _coroutine;


        [Button()]
        public void StartMusic() => _coroutine = StartCoroutine(PlayMusic());

        private void StopMusic()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            usersHolder.StateStop();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                StartMusic();
            else if (Input.GetKeyDown(KeyCode.H))
                StopMusic();
        }


        private IEnumerator PlayMusic()
        {
            do
            {
                var randomLength = UnityEngine.Random.Range(3, 8);
                usersHolder.StateStart();
                audioSource.Play();
                audioSource.loop = true;
                audioSource.pitch = Mathf.Lerp(0.9f, 1.15f, Random.value);
                yield return new WaitForSecondsRealtime(randomLength);
                usersHolder.StateStop();
                audioSource.Stop();

                yield return new WaitForSecondsRealtime(3);
            } while (_coroutine != null);
        }
    }
}