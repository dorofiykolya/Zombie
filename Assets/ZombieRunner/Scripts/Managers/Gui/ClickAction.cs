using UnityEngine;
using System.Collections;

namespace Runner
{
	public enum GUIAction
	{
		Pause,
		Settings,
		Missions,
		Leaderboard,
		Characters,
		Shop,
		Resume,
		Credits,
		Home,
		Back
	}
	
	[AddComponentMenu("Runner/GUI/ClickAction")]
	
	public class ClickAction : ComponentManager 
	{
		public GUIAction action;
		void OnClick()
		{
			switch(action)
			{
				case GUIAction.Pause:
					Player.isStop = true;
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.Missions).Show();
					GUIPanelManager.Get(PanelType.Missions).Adjust();
					GUIPanelManager.currentPanel = PanelType.Missions;
					break;
				case GUIAction.Settings:
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.Settings).Show();
					GUIPanelManager.currentPanel = PanelType.Settings;
					break;
				case GUIAction.Missions:
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.Missions).Show();
					GUIPanelManager.Get(PanelType.Missions).Adjust();
					GUIPanelManager.currentPanel = PanelType.Missions;
				break;
				case GUIAction.Leaderboard:
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.Leaderboard).Show();
					GUIPanelManager.currentPanel = PanelType.Leaderboard;
				break;
				case GUIAction.Characters:
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.Character).Show();
					GUIPanelManager.currentPanel = PanelType.Character;
					break;
				case GUIAction.Shop:
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.Shop).Show();
					GUIPanelManager.currentPanel = PanelType.Shop;
					break;
				case GUIAction.Resume:
					Player.isStop = false;
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.GameMenu).Show();
					GUIPanelManager.currentPanel = PanelType.GameMenu;
					break;
				case GUIAction.Credits:
					break;
				case GUIAction.Home:
					if( States.Current != State.LOAD)
					{
						Game.GameRestart();
					}
					break;
				case GUIAction.Back:
					GUIPanelManager.Get(GUIPanelManager.currentPanel).Hide();
					GUIPanelManager.Get(PanelType.MainMenu).Show();
					GUIPanelManager.currentPanel = PanelType.MainMenu;
					break;
			}
		}
	}
}