using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runner
{
    public class GameManager
    {
        public event Action OnStart;
        public event Action OnPause;
        public event Action OnStop;
        public event Action OnResume;
        public event Action OnRestart;
		private List<IComponentManager> components;

        public bool Paused { get; private set; }
        public bool Started { get; private set; }

		public GameManager(List<IComponentManager> components)
        {
            this.components = components;
        }

        public void GameStart()
		{
            Paused = false;
            Started = true;
            foreach (var component in components.ToArray())
            {
                component.GameStart();
            }
            if (OnStart != null)
            {
                OnStart.Invoke();
            }
        }

        public void GameStop()
        {
            Paused = false;
            Started = false;

			StorageManager.Save();

            foreach (var component in components.ToArray())
            {
                component.GameStop();
            }
            if (OnStop != null)
            {
                OnStop.Invoke();
            }
        }

        public void GamePause()
        {
            Paused = true;
            foreach (var component in components.ToArray())
            {
                component.GamePause();
            }
            if (OnPause != null)
            {
                OnPause.Invoke();
            }
        }

        public void GameResume()
        {
            Paused = false;
            foreach (var component in components.ToArray())
            {
                component.GameResume();
            }
            if (OnResume != null)
            {
                OnResume.Invoke();
            }
        }

        public void GameRestart()
        {
            GameStop();
            foreach (var component in components.ToArray())
            {
                component.GameRestart();
            }
            if (OnRestart != null)
            {
                OnRestart.Invoke();
            }
        }
    }
}
