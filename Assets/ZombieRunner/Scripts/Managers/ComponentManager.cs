using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner
{
	public interface IComponentManager
	{
		 void GameStart();
		 void GameStop();
		 void GamePause();
		 void GameResume();
		 void GameRestart();
	}

    public class ComponentManager : MonoBehaviour, IComponentManager
    {
        public MissionManager Missions { get; private set; }
        public QualityManager Quality { get; private set; }
        public PlayerManager Player { get; private set; }
        public TimerManager Timer { get; set; }
        public StateManager States { get; private set; }
        public CameraManager Cameras { get; private set; }
        public WaypointManager Waypoint { get; private set; }
        public GameManager Game { get; private set; }
        public LocationManager Location { get; private set; }
		public PowerUpManager PowerUp { get; private set; }
        public Manager Manager { get; private set; }

        public bool Initialized { get; private set; }

        void Awake()
        {
            Manager = Runner.Manager.Instance;
            Manager.Register(this);
            Game = Manager.Game;
            Location = Manager.Location;
            Missions = Manager.Missions;
            Quality = Manager.Quality;
            Player = Manager.Player;
            Timer = Manager.Timer;
            States = Manager.States;
            Cameras = Manager.CamerasManager;
            Waypoint = Manager.Waypoint;
			PowerUp = Manager.PowerUp;

            Initialize();
            Initialized = true;
        }

        public virtual void Initialize()
        { 
            
        }

        public virtual void GameStart()
        {
            
        }

        public virtual void GameStop()
        {
            
        }

        public virtual void GamePause()
        {
            
        }

        public virtual void GameResume()
        {
            
        }

        public virtual void GameRestart()
        {
            
        }
    }
}
