using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner
{
    public class GameController
    {
        private GameManager game;

        public GameController(GameManager game, Manager manager)
        {
            this.game = game;
            AddHandlers();
        }

        private void AddHandlers()
        {
            game.OnRestart += OnGameRestart;
        }

        private void OnGameRestart()
        {
            Time.timeScale = 1;
        }
    }
}
