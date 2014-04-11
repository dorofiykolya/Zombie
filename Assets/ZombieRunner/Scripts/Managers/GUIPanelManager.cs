using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Runner
{
	public enum PanelType
	{
		None,
		MainMenu,
		GameMenu,
		Settings,
		Shop,
		Character,
		Missions,
		Leaderboard
	}
	
	[AddComponentMenu("Runner/GUI/Panel")]
    public class GUIPanelManager : ComponentManager
	{
		private static Dictionary<PanelType, GUIPanelManager> dictionary = new Dictionary<PanelType, GUIPanelManager>();
		public static GUIPanelManager Get(PanelType panel)
		{
			GUIPanelManager result;
			if(dictionary.TryGetValue(panel, out result))
			{
				return result;
			}
			return null;
		}

		public static PanelType currentPanel = PanelType.MainMenu;
		public PanelType panel;
		
        public override void Initialize()
        {
            if (dictionary.ContainsKey(panel))
            {
                Debug.LogError("GUIPanelMenu is already exist with key:" + panel);
                return;
            }
            dictionary.Add(panel, this);

            if (panel != PanelType.MainMenu)
            {
                Player.isStop = true;
                gameObject.SetActive(false);
            }
        }
		
		public void Show()
		{
			gameObject.SetActive(true);
		}
		
		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}