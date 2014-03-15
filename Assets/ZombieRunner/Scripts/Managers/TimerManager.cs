using UnityEngine;
using System.Collections;
namespace Runner
{
	public class TimerManager : MonoBehaviour {
		
		
		public static event System.Action<float> OnTimer;
		
		private static float lastTime = 0;
		private static float timeScale = 1;
		
		/// <summary>
		/// Gets or sets the time scale (deltaTime * scale).
		/// </summary>
		/// <value>
		/// The scale.
		/// </value>
		public static float Scale
		{
			get{return timeScale;}
			set
			{
				if(timeScale < 0.0f)
				{
					timeScale = 0.0f;	
				}
				timeScale = value;
			}
		}
		
		public static void DefaultScale()
		{
			timeScale = 1.0f;
		}

		public static void Pause()
		{
			Time.timeScale = 0f;
		}

		public static void Play()
		{
			Time.timeScale = timeScale;
		}
		
		// Use this for initialization
		void Start () 
		{
			lastTime = Time.time;
		}
		
		// Update is called once per frame
		void Update () 
		{
			float newTime = Time.time;
			lastTime = newTime - lastTime;
			if(OnTimer != null)
			{
				OnTimer(lastTime * timeScale);
			}
			lastTime = newTime;
		}
	}
}