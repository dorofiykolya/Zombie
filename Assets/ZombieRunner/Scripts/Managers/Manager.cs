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
	public class Manager : MonoBehaviour {

		public static void Restart ()
		{
			LocationManager.Restart();
			StateManager.Current = State.GAME;
			PlayerManager.isStop = false;
			Time.timeScale = 1;
		}
		
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
	}
}
