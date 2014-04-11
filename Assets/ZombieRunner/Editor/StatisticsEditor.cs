using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Runner
{
	[CustomEditor(typeof(Runner.StatisticsManager))]
	[CanEditMultipleObjects]

	public class StatisticsEditor : UnityEditor.Editor {
	
		void OnEnable()
		{
			
		}
		
		public override void OnInspectorGUI ()
		{
			Separate();
			GUI.color = ColorEditor.Title;
			EditorGUILayout.LabelField("GameInfo Info", EditorStyles.boldLabel);
			GUI.color = Color.white;
				DrawInfo("CharacterId", PlayerData.CharacterId);
				DrawInfo("Distance", PlayerData.Distance);
				DrawInfo("TypeRemainingDistance", PlayerData.PlatformTypeRemainingDistance);
				DrawInfo("PlatformMode", PlayerData.PlatformType);
                DrawInfo("Speed", ((ComponentManager)target).Player.Speed);
			Separate();
			GUI.color = ColorEditor.Title;
			EditorGUILayout.LabelField("Platform Info", EditorStyles.boldLabel);
			GUI.color = Color.white;

                if (((ComponentManager)target).Location.Platforms != null)
				{
                    DrawInfo("In Location Platform Count:", ((ComponentManager)target).Location.Platforms.Count);
				}
                DrawInfo("In Dispose List", ((ComponentManager)target).Location.DisposedManager.Count);
		}
        
        private void Separate()
		{
			EditorGUILayout.Separator();	
		}
		
		private void DrawInfo(string label, object value)
		{
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
				EditorGUILayout.LabelField(value.ToString());
			EditorGUILayout.EndHorizontal();
		}
	}
}
