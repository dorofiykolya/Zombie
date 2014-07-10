using UnityEngine;
using System.Collections;

namespace Runner
{
	public class PoolManager
	{
		private static Hashtable dictionary = new Hashtable();
		
		public static object Pop(System.Type type)
		{
			return Pop(type, new System.Type[]{},new object[]{});
		}
		
		public static object Pop(System.Type type, System.Type[] constructorTypes, object[] constructorParameters)
		{
			ArrayList list = dictionary[type] as ArrayList;
			if(list == null)
			{
				return type.GetConstructor(constructorTypes).Invoke(constructorParameters);	
			}
			var result = list[0];
			list.RemoveAt(0);	
			return result;
		}
		
		public static void Push(Object obj)
		{
			var type = obj.GetType();
			ArrayList list = dictionary[type] as ArrayList;
			if(list == null)
			{
				list = new ArrayList();
				dictionary[type] = list;
			}
			list.Add(obj);
		}
	}
}

