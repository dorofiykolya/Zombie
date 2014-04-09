using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Runner
{
	[System.Serializable]
	public class PlatformInfo
	{
		public int type;
		public int distance;
		
		public PlatformInfo()
		{
			
		}
	}
	
	public class PlatformInfoManager : MonoBehaviour {
		
		public static List<PlatformInfo> List = new List<PlatformInfo>();
		private static Dictionary<int,int> Type;
		
		void Awake()
		{
			if(Type == null)
			{
				Type = new Dictionary<int, int>(List.Count);	
			}
			foreach(var current in List)
			{
				Type[current.type] = current.distance;	
			}
		}
		
		public static int GetDistanceByType(int type)
		{
			if(Type == null)
			{
				Type = new Dictionary<int, int>(List.Count);	
				foreach(var current in List)
				{
					Type[current.type] = current.distance;	
				}
			}
			return Type[type];	
		}
	}
}