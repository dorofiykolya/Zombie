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
		Leaderboard,
		Lose,
        Map
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

            if (panel != PanelType.MainMenu && panel != PanelType.Map)
            {
                Player.isStop = true;
                gameObject.SetActive(false);
            }

            if (panel == PanelType.Map)
            {
                transform.GetChild(0).GetComponent<UIPanel>().clipOffset = new Vector2(0, -5000);
                transform.GetChild(0).GetComponent<UIPanel>().transform.localPosition = new Vector3(0, 5000, 0);
            }
        }

		public void Adjust()
		{
			if(panel == PanelType.Missions)
			{
				transform.FindChild(State.GAME.ToString()).gameObject.SetActive(States.Current == State.GAME || States.Current == State.LOSE);
			}
			else if(panel == PanelType.Shop)
			{
				transform.FindChild(State.LOAD.ToString()).gameObject.SetActive(States.Current == State.LOAD);
			}
			else if(panel == PanelType.Leaderboard)
			{
				transform.FindChild(State.LOAD.ToString()).gameObject.SetActive(States.Current == State.LOAD);
				transform.FindChild(State.GAME.ToString()).gameObject.SetActive(States.Current == State.GAME || States.Current == State.LOSE);
			}
		}
		
		public void Show()
		{
			gameObject.SetActive(true);

            if (panel == PanelType.MainMenu)
            {
                Get(PanelType.Map).gameObject.SetActive(true);
            }
		}
		
		public void Hide()
		{
			gameObject.SetActive(false);
            if (panel == PanelType.MainMenu)
            {
                Get(PanelType.Map).gameObject.SetActive(false);
            }
		}
	}
}