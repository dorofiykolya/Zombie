using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Runner
{
	public delegate object GetData();
	public class StatisticsData
	{
		public StatisticsData(string label, GetData d)
		{
			this.label = label;
			this.getValue = d;
		}
		public string label;
		public GetData getValue;
	}
	
	public class StatisticsManager : MonoBehaviour
	{
		
		private static bool IsEditor = Application.isEditor;
		private static List<StatisticsData> list = new List<StatisticsData>();
		public float speed = 0.0f;
		
		void Start()
		{
				
		}
		
		void Add(string label, GetData getValue)
		{
			list.Add(new StatisticsData(label, getValue));	
		}
		
		void Update ()
		{
			if(IsEditor)
			{
				speed = PlayerManager.Speed;
			}
		}
	}
}