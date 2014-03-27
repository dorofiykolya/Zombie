using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Runner
{
	public class LocationPlatformManager
	{
		private static Runner.PlatformObject[] list;
		private static Runner.PlatformObject[] startList;
		private static Runner.PlatformInfo[] infoList;
		private static Dictionary<int,float> typeDistance;
		private static List<KeyValuePair<int, float>> typeSortedList;
		private static Vector3[] listSize;
		private static Runner.PlatformObject[] ListById;
		private static Dictionary<int,List<Runner.PlatformObject>> PoolById = new Dictionary<int, List<Runner.PlatformObject>>();
		private static Dictionary<int,Dictionary<int,Runner.PlatformObject>> transitionByType = new Dictionary<int, Dictionary<int, Runner.PlatformObject>>();
		
		private static void ParseSize(Runner.PlatformObject[] list)
		{
			foreach (Runner.PlatformObject p in list)
			{
				var render = p.gameObject.renderer;
				var size = Vector3.zero;
				if(render == null)
				{
					List<Renderer> rList = new List<Renderer>();
					var children = p.gameObject.GetChildren();
					
					var renders = from GameObject rl in children
						where rl.renderer != null
							select rl.renderer;
					
					foreach(Component r in renders)
					{
						if(size == Vector3.zero)
						{
							size = ((Renderer)r).bounds.size;
						}
						else
						{
							var s = ((Renderer)r).bounds.size;
							if(s.x > size.x) size.x = s.x;
							if(s.y > size.y) size.y = s.y;
							if(s.z > size.z) size.z = s.z;
						}
					}
				}
				else
				{
					size = p.gameObject.renderer.bounds.size;
				}
				p.Size = size;
				listSize[p.Id] = size;
			}
		}

        public static void ParsePlatforms(Runner.PlatformObject[] platforms, Runner.PlatformObject[] startPlatforms, Runner.PlatformObject[] transitionPlatforms)
		{
			Runner.LocationPlatformManager.list = platforms;
			Runner.LocationPlatformManager.startList = startPlatforms;
			ListById = new Runner.PlatformObject[platforms.Length + startPlatforms.Length + transitionPlatforms.Length];
			listSize = new Vector3[ListById.Length];
			Runner.PlatformObject current = null;
			int index = 0;
			for(var i = 0; i < startPlatforms.Length; i++, index++)
			{
				current = startPlatforms[i];
				current.Id = index;
				current.IsStartPlatform = true;
				ListById[index] = current;	 
			}
			for(var i = 0; i < platforms.Length; i++, index++)
			{
				current = platforms[i];
				current.Id = index;
				ListById[index] = current;	 
			}
			for(var i = 0; i < transitionPlatforms.Length; i++, index++)
			{
				current = transitionPlatforms[i];
			    if (current.Mode != PlatformMode.Transition)
			    {
                    throw new Exception("platform.Mode != Platform.Transition in transition list");
			        return;
			    }
				current.Id = index;
				ListById[index] = current;
			}
			ParseSize(startPlatforms);
			ParseSize(platforms);
			ParseSize(transitionPlatforms);
			
			ParsePlatformTransition(transitionPlatforms);
		}

        private static void ParsePlatformTransition(Runner.PlatformObject[] list)
		{
			var checkList = new List<int>();
			Dictionary<int, Runner.PlatformObject> outDic;
			Runner.PlatformObject current = null;
			foreach(var p in list)
			{
				if(transitionByType.TryGetValue(p.Type, out outDic) == false)
				{
					outDic = new Dictionary<int, PlatformObject>();
					transitionByType.Add(p.Type, outDic);
				}
				if(outDic.TryGetValue(p.TypeTo, out current))
				{
					ErrorManager.Show("Error", string.Format("LocationManager ParsePlatformTransition, type:{0}, typeTo:{1} - exist already", p.Type, p.TypeTo));
					return;	
				}
				
				if(checkList.Contains(p.Type) == false)
				{
					checkList.Add(p.Type);	
				}
				
				outDic.Add(p.TypeTo, p);
			}
			
			var checkResult = CheckTransitionCount(checkList.Count);
			if(checkList.Count != list.Length)
			{
				ErrorManager.Show("Error", string.Format("LocationManager ParsePlatformTransition list is not correct"));
				return;
			}
		}
		
		private static int CheckTransitionCount(int count)
		{
			if(count <= 1)
			{
				return count;	
			}
			return count * CheckTransitionCount(count - 1);
		}
		
		public static void ParseInfo(Runner.PlatformInfo[] info)
		{
			infoList = info;
			typeDistance = new Dictionary<int, float>(info.Length);
			foreach(var i in info)
			{
				if(typeDistance.ContainsKey(i.type))
				{
					ErrorManager.Show("Error", "PlatformInfoManager. Check Platform Info, duplicate of type:" + i.type);
					return;	
				}
				typeDistance.Add(i.type, i.distance);
			}
			typeSortedList = typeDistance.ToList();
			typeSortedList.Sort((s,t)=>
				{
					if(s.Value < t.Value) return -1;
					if(s.Value > t.Value) return 1;
					return 0;	
				}
			);
		}
		
		public static int GetTypeByDistance(float distance, int type, bool random = false)
		{
			var len = typeSortedList.Count;
			var i = len;
			cicle : while(i-- > 0)
			{
				if(typeSortedList[i].Value <= distance)
				{
					if(random && typeSortedList[i].Key == type)
					{
						continue;	
					}
					return typeSortedList[i].Key;	
				}
			}
			
			if(random)
			{
				random = false;
				i = len;
				goto cicle;
			}
			
			return -1;
		}
		
		public static float GetDistanceByType(int type)
		{
			return typeDistance[type];	
		}
		
		public static Vector3 GetSize(Runner.PlatformObject p)
		{
			return (Vector3)listSize[p.Id];
		}
		
		public static Vector3 GetSize(int id)
		{
			return (Vector3)listSize[id];
		}
		
		public static void GetSize(out Vector3 result, ref int id)
		{
			result = (Vector3)listSize[id];
		}
		
		public static void GetSize(out Vector3 result, Runner.PlatformObject p)
		{
			result = (Vector3)listSize[p.Id];
		}
		
		public static Runner.PlatformObject GetTransitionPlatform(int typeFrom, int typeTo)
		{
			Dictionary<int, Runner.PlatformObject> current;
			Runner.PlatformObject platf;
			if(transitionByType.TryGetValue(typeFrom, out current))
			{
				if(current.TryGetValue(typeTo, out platf))
				{
					return PopPlatform(platf);
				}
				return null;
			}
			return null;
		}
		
		public static Runner.PlatformObject GetRandomStartPlatform(int type = 0)
		{
			int len = startList.Length;
			if(len == 1)
			{
				return PopPlatform(startList[0]);	
			}
			else if(len > 0)
			{
				var resultList = from p in startList
					where p.Type == type
						select p;
				var result = resultList.ToArray();
				if(result.Length != 0)
				{
					return PopPlatform(result[Random.Range(0, result.Length)]);
				}
				return PopPlatform(startList[Random.Range(0, len)]);	
			}
			return null;	
		}
		
		public static Runner.PlatformObject GetRandomPlatformByMinimumDistance(float distance)
		{
			var result = from p in list
				where p.MinimumDistance <= distance
					select p;
			
			var len = result.Count();
			if(len == 1)
			{
				return PopPlatform(result.ElementAt(0).Id);	
			}
			else if(len > 0)
			{
				return PopPlatform(result.ElementAt(Random.Range(0, len)).Id);	
			}
			
			return null;
		}
		
		public static Runner.PlatformObject GetRandomPlatformByTypeAndDistance(int type, float distance)
		{
			var result = from p in list
				where p.MinimumDistance <= distance && p.Type == type
					select p;
			
			
			var len = result.Count();
			if(len == 1)
			{
				return PopPlatform(result.ElementAt(0).Id);	
			}
			else if(len > 0)
			{
				return PopPlatform(result.ElementAt(Random.Range(0, len)).Id);	
			}
			return null;	
		}
		
		public static Runner.PlatformObject PopPlatform(int id)
		{
			List<Runner.PlatformObject> array;
			Runner.PlatformObject result = null;
			int count;
			if(PoolById.TryGetValue(id, out array))
			{
				count = array.Count;
				if(count > 0)
				{
					result = (Runner.PlatformObject)array[count - 1];
					array.RemoveAt(count - 1);
				}
			}
			if(result == null)
			{
				result = ListById[id].Clone();
			}
			result.gameObject.SetActive(false);
			result.SetStartTransform();
			return result;
		}
		
		public static void PushPlatform(Runner.PlatformObject p)
		{
			List<Runner.PlatformObject> array;
			if(PoolById.TryGetValue(p.Id, out array) == false)
			{
				array = new List<PlatformObject>();
				PoolById.Add(p.Id, array);
			}
			p.gameObject.SetActive(false);
			array.Add(p);
		}
		
		public static Runner.PlatformObject PopPlatform(Runner.PlatformObject p)
		{
			return PopPlatform(p.Id);	
		}
	}
}

