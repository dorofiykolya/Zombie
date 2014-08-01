using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runner
{
	public class MissingDataEditor : EditorWindow 
	{
		private GameObject[] selectList;
		private Vector2 scrollPosition;

		private GameObject copiedComponent;

		void OnGUI()
		{
			copiedComponent = EditorGUILayout.ObjectField (copiedComponent, typeof(GameObject)) as GameObject;

			if (GUILayout.Button("ADD"))
			{
				AddAnimation();
			}

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			EditorGUILayout.Separator();
			GUILayout.BeginHorizontal();
			GUILayout.Space(30.0f);
			GUILayout.BeginVertical();
			GUILayout.Box("SELECT LIST");
			
			EditorGUILayout.Separator();
			selectList = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets).Cast<GameObject>().ToArray();
			GUI.color = Color.green;
			
			EditorGUILayout.Separator();
			if (selectList == null || selectList.Length == 0)
			{
				GUILayout.Label("empty select list");
			}
			else
			{
				foreach (var o in selectList)
				{
					GUILayout.Label(o.name);
				}
			}
			GUI.color = Color.white;
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			EditorGUILayout.EndScrollView();
		}

		private void AddAnimation()
		{
			if (selectList == null || selectList.Length == 0) return;
			foreach (var l in selectList)
			{
				Adjustment(l);
			}
		}

		[MenuItem("Runner/Prefabs Adjustment")]
		public static void ShowShaderQualityWindowEditor()
		{
			EditorWindow.GetWindow<MissingDataEditor>("Prefabs Adjustment", true).Show();
		}

		private void Adjustment(GameObject gameObject) 
		{
			if(gameObject.name.Contains("Human"))
			{
				if(gameObject.transform.localPosition.x > 4 && gameObject.transform.localPosition.x < 12)
				{
					gameObject.transform.localPosition = new Vector3(8, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				else if(gameObject.transform.localPosition.x > -4 && gameObject.transform.localPosition.x < 4)
				{
					gameObject.transform.localPosition = new Vector3(0, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				else if(gameObject.transform.localPosition.x > -12 && gameObject.transform.localPosition.x < -4)
				{
					gameObject.transform.localPosition = new Vector3(-8, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				Adjustment(child.gameObject);
			}
		}
	}
}
