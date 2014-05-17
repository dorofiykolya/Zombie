using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Runner
{
	[RequireComponent(typeof(Runner.TimerManager))]
	[RequireComponent(typeof(Runner.CameraManager))]
	[RequireComponent(typeof(Runner.PlayerManager))]
	[RequireComponent(typeof(Runner.LocationManager))]
	[RequireComponent(typeof(Runner.InputManager))]
	[RequireComponent(typeof(Runner.StateManager))]
	[RequireComponent(typeof(Runner.EffectManager))]
	[RequireComponent(typeof(Runner.PlatformInfoManager))]
	[RequireComponent(typeof(Runner.StatisticsManager))]
	[RequireComponent(typeof(Runner.WaypointManager))]
    [RequireComponent(typeof(Runner.SettingManager))]
    [RequireComponent(typeof(Runner.MissionManager))]
    [RequireComponent(typeof(Runner.StorageManager))]
    [RequireComponent(typeof(Runner.QualityManager))]
	[RequireComponent(typeof(Runner.PowerUpManager))]

	public class Manager : MonoBehaviour 
    {
        public static Manager Instance { get; private set; }

        public MissionManager Missions { get; private set; }
        public QualityManager Quality { get; private set; }
        public PlayerManager Player { get; private set; }
        public TimerManager Timer { get; set; }
        public StateManager States { get; private set; }
        public CameraManager CamerasManager { get; private set; }
        public WaypointManager Waypoint { get; private set; }
        public GameManager Game { get; private set; }
        public LocationManager Location { get; private set; }
		public PowerUpManager PowerUp { get; private set; }

		private List<IComponentManager> components = new List<IComponentManager>();
        private GameController gameController;

	    void Awake()
	    {
	        Instance = this;
            Game = new GameManager(components);
	        Missions = GetComponent<MissionManager>();
	        Quality = GetComponent<QualityManager>();
	        Player = GetComponent<PlayerManager>();
	        Timer = GetComponent<TimerManager>();
	        States = GetComponent<StateManager>();
	        CamerasManager = GetComponent<CameraManager>();
            Location = GetComponent<LocationManager>();
            Waypoint = GetComponent<WaypointManager>();
			PowerUp = GetComponent<PowerUpManager>();
            gameController = new GameController(Game, this);

			var componentManagers = GetComponents<ComponentManager>();
            foreach (var c in componentManagers)
            {
                Register(c);
            }
	    }

		/*public static void Restart ()
		{
			//LocationManager.Restart();
			StateManager.Current = State.GAME;
			PlayerManager.isStop = false;
			Time.timeScale = 1;
		}*/
		
		

		void Start () {
			
		}
		

		void Update () {
			
		}


		internal void Register(IComponentManager componentManager)
        {
            if(componentManager.Initialized) return;
            components.Add(componentManager);
        }
    }
}
