﻿using UnityEngine;
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
				Missions.Dispatch(mission.Id, mission.Target);
				Game.GamePause();
			}

			Audio.PlaySound (12);
		}
	}
}
