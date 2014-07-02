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
		}

		public static bool SetBrains(int value)
		{
			if(BrainsAmount + value < 0)
			{
				GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
				GUIPanelManager.Get(PanelType.Shop).Show();
				GUIPanelManager.Get(PanelType.Shop).Adjust();
				GUIPanelManager.currentPanel = PanelType.Shop;
				return false;
			}
			BrainsAmount += value; 
			if(OnChanged != null)
			{
				OnChanged(BrainsAmount.ToString());	
			}

			return true;
		}
    }
}
