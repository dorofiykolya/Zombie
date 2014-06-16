using UnityEngine;
using System.Collections;
using System;


namespace Runner
{
    [Serializable]
	public class PlayerData
	{
		[SerializeField]
		public static float Distance = 0;
		[SerializeField]
		public static int Brains = 0;
		[SerializeField]
		public static int CharacterId = 0;
		[SerializeField]
		public static int PlatformType = 0;
		[SerializeField]
		public static float PlatformTypeRemainingDistance = 0;
		[SerializeField]
		public static float MaxDistance = 0;
    }
}
