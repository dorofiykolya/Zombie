using UnityEngine;
using System.Collections;

namespace Runner
{
	public static class GameUtils
	{
		public static void AddChild(this GameObject instance, GameObject child)
		{
			child.transform.parent = instance.transform;
		}
		
		public static void RemoveFromParent(this GameObject instance)
		{
			instance.transform.parent = null;	
		}
		
		public static int NumChildren(this GameObject instance)
		{
			return instance.transform.childCount;
		}
		
		public static void RemoveAllChildren(this GameObject o)
		{
			o.transform.DetachChildren(); 	
		}
		
		public static GameObject[] GetChildren(this GameObject o)
		{
			GameObject[] list = new GameObject[o.transform.childCount];
			for(int i = 0; i < o.transform.childCount; i++)
			{
				list[i] = o.transform.GetChild(i).gameObject;	
			}
			return list;
		}
		
		public static GameObject GetChild(this GameObject o, int index)
		{
			return o.transform.GetChild(index).gameObject;	
		}
		
		public static float Distance(this Vector3 a, ref Vector3 vector)
		{
			return Vector3.Distance(a,vector);
		}
		
		public static float Distance(this Vector3 a, Vector3 vector)
		{
			return Vector3.Distance(a,vector);
		}
		
		public static void Destroy(this GameObject o)
		{
			GameObject.DestroyObject(o);
		}
		
		public static GameObject Clone(this GameObject o)
		{
			return	(GameObject)GameObject.Instantiate(o);
		}
		
		public static GameObject Clone(this GameObject o, Vector3 position, Quaternion rotation)
		{
			return (GameObject)GameObject.Instantiate(o, position, rotation);
		}
		
		public static void CopyFrom(this Vector3 v, ref Vector3 copyFrom)
		{
			v.x = copyFrom.x;
			v.y = copyFrom.y;
			v.z = copyFrom.z;
		}
		
		public static void Offset(this Vector3 v, ref Vector3 offset)
		{
			v.x += offset.x;
			v.y += offset.y;
			v.z += offset.z;
		}
	}
}
