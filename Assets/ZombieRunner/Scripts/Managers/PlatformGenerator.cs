using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Runner
{
	public class PlatformGenerator
	{
		private static LocationStack stack = new LocationStack();
		private static Vector3 vectorHelper = new Vector3(0,0,0);
		private static PlatformObject next;
		
		public static void Reset()
		{
			stack.Reset();	
			next = null;
		}
			
		public static void Generate(float speed, Runner.PlayerController player)
		{
			Runner.PlatformObject platform = LocationManager.Platforms.Last;
			bool isStartPlatform = false;
			if(platform == null)
			{
				platform = LocationPlatformManager.GetRandomStartPlatform(Runner.PlayerData.PlatformType);
				isStartPlatform = true;
				if(platform == null)
				{
					ErrorManager.Show("empty start platform", "add start platforms");
					return;
				}
				PlayerData.PlatformTypeRemainingDistance = Runner.LocationPlatformManager.GetDistanceByType(platform.Type);
				next = platform.GetNextRandom();
				LocationManager.Platforms.Add(platform);
			}
			float distance = platform.Distance(player);
			Vector3 size;
			int count = 0;
			while(distance <= LocationManager.GenerateDistance && count < Runner.LocationManager.MaxGeneratePlatforms)
			{
				int nextType = -1;
				if(PlayerData.PlatformTypeRemainingDistance <= 0.0f && isStartPlatform == false)
				{
					nextType = LocationPlatformManager.GetTypeByDistance(player.Distance, PlayerData.PlatformType, Random.Range(0,2) > 0);
					if(nextType == -1)
					{
						nextType = platform.Type;	
					}
					if(nextType != PlayerData.PlatformType)
					{
						next = LocationPlatformManager.GetTransitionPlatform(PlayerData.PlatformType, nextType);
					}
					if(next == null)
					{
						next = LocationPlatformManager.GetRandomPlatformByTypeAndDistance(PlayerData.PlatformType, Runner.PlayerManager.Distance);
					}
					PlayerData.PlatformType = nextType;
					PlayerData.PlatformTypeRemainingDistance += LocationPlatformManager.GetDistanceByType(nextType);
				}
				else
				{
					next = next != null? next : LocationPlatformManager.GetRandomPlatformByTypeAndDistance(PlayerData.PlatformType, Runner.PlayerManager.Distance);
				}
				LocationPlatformManager.GetSize(out size, platform);
				vectorHelper.x = 0;
				vectorHelper.y = 0;
				vectorHelper.z = size.z - size.z / 2;
				next.transform.position = platform.transform.position + vectorHelper;
				platform = next;
				LocationPlatformManager.GetSize(out size, platform);
				vectorHelper.x = 0;
				vectorHelper.y = 0;
				vectorHelper.z = size.z - size.z / 2;
				platform.transform.position += vectorHelper;
				distance = platform.Distance(player);
				LocationManager.Platforms.Add(platform);
				next = platform.GetNextRandom();
				count++;
			}
		}
		
		public static void Move(float speed, Runner.PlayerController player)
		{
			var result = from c in LocationManager.Platforms.List
				where c.transform.position.z < player.transform.position.z
					select c;
			
			foreach(var current in result)
			{
				current.AllowDispose = true;
				LocationDisposeManager.Add(current);	
			}
		}
		
	}
}

