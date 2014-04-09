using UnityEngine;
using System.Collections;
using UnityEditor;
using Runner;
namespace Runner
{
	[CustomEditor(typeof(PlatformInfoManager))]
	[CanEditMultipleObjects]
	public class PlatformTypeEditor : Editor
	{
		private Runner.LocationManager manager;
		private bool changed;
	
		void OnEnable()
		{
			manager = (Runner.LocationManager)GameObject.FindObjectOfType(typeof(Runner.LocationManager));
			if(manager == null)
			{
				ErrorManager.Show("Error","PlatformTypeEditor, manager == null");
				return;
			}
			if(manager.platformsInfo != null)
			{
				PlatformInfoManager.List.Clear();
				PlatformInfoManager.List.AddRange(manager.platformsInfo);
			}
			changed = true;
		}
		
		public override void OnInspectorGUI ()
		{
			Draw();
			GUI.color = ColorEditor.Title;
			if(GUILayout.Button("Add"))
			{
				PlatformInfoManager.List.Add(new Runner.PlatformInfo());	
				manager.platformsInfo = PlatformInfoManager.List.ToArray();
			}
			GUI.color = Color.white;
			if(changed)
			{
				Runner.LocationManager manager = (Runner.LocationManager)GameObject.Find("Game").GetComponent(typeof(Runner.LocationManager));
				manager.platformsInfo = PlatformInfoManager.List.ToArray();
				changed = false;
			}
		}
	
		private void Draw()
		{
			var list = PlatformInfoManager.List;
			int type;
			int distance;
			if(list != null)
			{
				int i = 0;
				foreach(var current in list)
				{
					GUI.color = ColorEditor.Title;
					EditorGUILayout.LabelField(string.Format("Info {0}", i));
					GUI.color = Color.white;
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Type", GUILayout.Width(35));
					type = EditorGUILayout.IntField(current.type);
					EditorGUILayout.LabelField("Distance", GUILayout.Width(55));
					distance = EditorGUILayout.IntField(current.distance);
				
					if(
						type != current.type 
						|| distance != current.distance
					)
					{
						current.type = type;
						current.distance = distance;
						changed = true;
					}
					
					GUI.color = Color.red;
					if(GUILayout.Button(new GUIContent("", "Remove"), GUILayout.Width(12), GUILayout.Height(12)))
					{	
						list.Remove(current);
						changed = true;
						return;
					}
					GUI.color = Color.white;
					EditorGUILayout.EndHorizontal();
					i++;
				}
			}
		}
	}
}