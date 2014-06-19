using UnityEngine;
using System.Collections;
using System;


namespace Runner
{
    [Serializable]
	public class PlayerData
	{
		public static float Distance = 0;
		private static int BrainsAmount = 0;
		public static int CharacterId = 0;
		public static int PlatformType = 0;
		public static float PlatformTypeRemainingDistance = 0;
		public static float MaxDistance = 0;
		public static int missionMulti = 0;

		public static event System.Action<string> OnChanged;

		public static int Brains
		{
			get { return BrainsAmount; }
			set {
					BrainsAmount = value; 
					if(OnChanged != null)
					{
						OnChanged(value.ToString());	
					}
				}
		}
    }
}
