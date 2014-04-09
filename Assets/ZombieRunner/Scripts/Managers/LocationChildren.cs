using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Runner
{
	public class LocationChildren
	{
		private Vector3 vectorHelper = new Vector3(0,0,0);
		
		private List<Runner.PlatformObject> list = new List<Runner.PlatformObject>(64);
		
		public Runner.PlatformObject Last{get;private set;}
		public List<Runner.PlatformObject> List {get{return list;}}
		
		public int Count{get{return list.Count;}}
		
		public void Add(Runner.PlatformObject platform)
		{
			if(list.Contains(platform) == false)
			{
				list.Add(platform);
				platform.InPlatformList = true;
				if(Last == null)
				{
					Last = platform;
				}
				else if(platform.transform.position.z > Last.transform.position.z)
				{
					Last = platform;	
				}
				platform.gameObject.SetActive(true);
				LocationManager.PlatformContainer.AddChild(platform.gameObject);
			}
		}
		
		public void Remove(Runner.PlatformObject platform)
		{
            //ObstacleManager.RemovePlatformObstacle(platform);

			if(list.Remove(platform))
			{
				platform.InPlatformList = false;	
			}
			Runner.LocationPlatformManager.PushPlatform(platform);
			platform.gameObject.SetActive(false);
			LocationManager.DisposedPlatformContainer.AddChild(platform.gameObject);
		}
		
		public void Move(Vector3 move)
		{
			var len = list.Count;
			Runner.PlatformObject p;
			for(var i = 0; i < len; i++)
			{
				p = list[i];
				p.Move(move);
			}
		}
		
		public void Move(float move, Runner.PlayerController player)
		{
			vectorHelper.Set(0,0,-move);
			var len = list.Count;
			Runner.PlatformObject p;
			float distance;
			for(var i = 0; i < len; i++)
			{
				p = list[i];
				p.Move(vectorHelper);
				distance = p.Distance(player);
			}
		}
		
		public void RemoveAll()
		{
			foreach(var platform in list)
			{
				LocationManager.DisposedPlatformContainer.AddChild(platform.gameObject);
				Runner.LocationPlatformManager.PushPlatform(platform);
				platform.InPlatformList = false;
				platform.AllowDispose = false;
				platform.gameObject.SetActive(false);
			}
			list.Clear();
			Last = null;
		}
	}
}

