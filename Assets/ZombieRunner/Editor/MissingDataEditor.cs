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

		private GameObject coin;

		void OnGUI()
		{
			coin = EditorGUILayout.ObjectField ("Coin", coin, typeof(GameObject)) as GameObject;

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
			if(gameObject.name == "Human5")
			{
				(gameObject.GetComponent<ObstacleHuman>().movement.collider as SphereCollider).radius = 20;
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				Adjustment(child.gameObject);
			}
		}
	}
}
