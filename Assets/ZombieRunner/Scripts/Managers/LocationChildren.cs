using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Runner
{
	public class LocationChildren
	{
		private Vector3 vectorHelper = new Vector3(0,0,0);
		
		private List<Runner.PlatformObject> list = new List<Runner.PlatformObject>(64);
        private GameObject platformContainer;
        private GameObject disposedPlatformContainer;
		
		public Runner.PlatformObject Last{get;private set;}
		public List<Runner.PlatformObject> List {get{return list;}}
		
		public int Count{get{return list.Count;}}

        public LocationChildren(GameObject PlatformContainer, GameObject DisposedPlatformContainer)
        {
            this.platformContainer = PlatformContainer;
            this.disposedPlatformContainer = DisposedPlatformContainer;
        }
		
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
				platformContainer.AddChild(platform.gameObject);
				PowerUpManager.AddPowerUpObjects(platform);
			}
		}
		
		public void Remove(Runner.PlatformObject platform)
		{
			if(list.Remove(platform))
			{
				platform.InPlatformList = false;	
			}
			Runner.LocationPlatformManager.PushPlatform(platform);
			platform.gameObject.SetActive(false);
			disposedPlatformContainer.AddChild(platform.gameObject);
			PowerUpManager.RemovePowerUpObjects (platform);
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
				disposedPlatformContainer.AddChild(platform.gameObject);
				Runner.LocationPlatformManager.PushPlatform(platform);
				platform.InPlatformList = false;
				platform.AllowDispose = false;
				platform.gameObject.SetActive(false);
				PowerUpManager.RemovePowerUpObjects (platform);
			}
			list.Clear();
			Last = null;
		}
	}
}

