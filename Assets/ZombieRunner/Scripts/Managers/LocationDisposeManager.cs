using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Runner
{
	public class LocationDisposeManager
	{
		private List<Runner.PlatformObject> list = new List<Runner.PlatformObject> ();
        private LocationChildren children;
		public int Count{get{return list.Count;}}

        public LocationDisposeManager(LocationChildren children)
        {
            this.children = children;
        }
		
		public void Add(Runner.PlatformObject item)
		{
			if (item.AllowDispose) {
				if (list.Contains (item) == false) {
					list.Add (item);	
				}
			}
		}

		public void RemoveAll()
		{
			list.Clear();
		}
		
		public void Update (Runner.PlayerController player)
		{
            List<Runner.PlatformObject> collection = children.List;
			var len = collection.Count;
			Runner.PlatformObject platform = null;
			var distance = player.gameObject.transform.position;
			for(var i = 0; i < len; i++)
			{
				platform = collection[i];
				if(platform.AllowDispose == false)
				{
					if(platform.transform.position.z < distance.z)
					{
						platform.AllowDispose = true;
						Add(platform);
					}
					else
					{
						break;	
					}
				}
			}
			
			len = list.Count;
			var index = 0;
			float currentDistance;
			float halfPlatformSize;
			while(index < len)
			{
				platform = list[index];
				currentDistance = Mathf.Abs(platform.transform.position.Distance(ref distance));
				halfPlatformSize = platform.Size.z / 2;
				if(currentDistance >= LocationManager.DisposeDistance && currentDistance > halfPlatformSize * LocationManager.MinDisposeMultiply)
				{
					index++;
					platform.AllowDispose = false;
                    children.Remove(platform);
				}
				else
				{
					break;
				}
			}
			
			if(index > 0)
			{
				list.RemoveRange(0, index);	
			}
		}
	}
	
}