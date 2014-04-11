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
        private Manager manager;

        public GameController(GameManager game, Manager manager)
        {
            this.game = game;
            this.manager = manager;
            AddHandlers();
        }

        private void AddHandlers()
        {
            game.OnRestart += OnGameRestart;
        }

        private void OnGameRestart()
        {

            //StateManager.Current = State.GAME;
            manager.Player.isStop = false;
            Time.timeScale = 1;
        }
    }
}
