using System;
using Models;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private StartScreen startScreen;

        [SerializeField] private GameController gameController;

        private void Start() => OpenStartWindow(gameController.StartGame);

        public void OpenStartWindow(Action startGameCallback) =>
            startScreen.Show(startGameCallback);

        public void OpenLoseWindow(LoseModel[] loseModels)
        {
        }

        public void CloseLoseWindow()
        {
        }
    }
}