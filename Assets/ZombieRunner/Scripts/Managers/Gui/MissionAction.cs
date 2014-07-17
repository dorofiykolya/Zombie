using UnityEngine;
using System.Collections;
namespace Runner
{
	public class MissionAction : ComponentManager 
	{
		[HideInInspector]
		public Mission mission;
		// Update is called once per frame
		void OnClick()
		{
			if(mission != null)
			{
				if(PlayerData.SetBrains(-5000))
				{
					Missions.Dispatch(mission.Id, mission.Target);
					Game.GamePause();
				}
			}

			Audio.PlaySound (12);
		}
	}
}
