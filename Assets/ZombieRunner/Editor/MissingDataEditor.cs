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
		private GameObject go1;
		private GameObject go2;
		private GameObject go3;
		private GameObject go4;
		private GameObject go5;

		private Material mat;
		private Mesh mesh;

		void OnGUI()
		{
			/*go1 = EditorGUILayout.ObjectField ("Human", go1, typeof(GameObject)) as GameObject;
			go2 = EditorGUILayout.ObjectField ("Human", go2, typeof(GameObject)) as GameObject;
			go3 = EditorGUILayout.ObjectField ("Human", go3, typeof(GameObject)) as GameObject;
			go4 = EditorGUILayout.ObjectField ("Human", go4, typeof(GameObject)) as GameObject;
			go5 = EditorGUILayout.ObjectField ("Human", go5, typeof(GameObject)) as GameObject;*/

			mat = EditorGUILayout.ObjectField ("Material", mat, typeof(Material)) as Material;
			mesh = EditorGUILayout.ObjectField ("Mesh", mesh, typeof(Mesh)) as Mesh;

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
				ChangeGold(l);
			}
		}

		[MenuItem("Runner/Prefabs Adjustment")]
		public static void ShowShaderQualityWindowEditor()
		{
			EditorWindow.GetWindow<MissingDataEditor>("Prefabs Adjustment", true).Show();
		}

		private void ChangeGold(GameObject gameObject)
		{
			if(gameObject.name == "probirka")
			{
				gameObject.renderer.sharedMaterial = mat;
				gameObject.GetComponent<MeshFilter>().mesh = mesh;
			}

			foreach (Transform child in gameObject.transform) 
			{
				ChangeGold(child.gameObject);
			}
		}

		private void Adjustment(GameObject gameObject) 
		{
			if (gameObject.CompareTag("Human") && gameObject.name == go1.name)
			{
				Vector3 position = gameObject.transform.localPosition;
				var parent = gameObject.transform.parent;

				DestroyImmediate(gameObject, true);

				var human = Instantiate(go1) as GameObject;
				human.name = human.name.Replace("(Clone)", "");
				human.transform.parent = parent;
				human.transform.localPosition = position;
				human.transform.localEulerAngles = new Vector3(0, 180, 0);

				return;
			}

			if (gameObject.CompareTag("Human") && gameObject.name == go2.name)
			{
				Vector3 position = gameObject.transform.localPosition;
				var parent = gameObject.transform.parent;
				
				DestroyImmediate(gameObject, true);
				
				var human = Instantiate(go2) as GameObject;
				human.name = human.name.Replace("(Clone)", "");
				human.transform.parent = parent;
				human.transform.localPosition = position;
				human.transform.localEulerAngles = new Vector3(0, 180, 0);
				
				return;
			}

			if (gameObject.CompareTag("Human") && gameObject.name == go3.name)
			{
				Vector3 position = gameObject.transform.localPosition;
				var parent = gameObject.transform.parent;
				
				DestroyImmediate(gameObject, true);
				
				var human = Instantiate(go3) as GameObject;
				human.name = human.name.Replace("(Clone)", "");
				human.transform.parent = parent;
				human.transform.localPosition = position;
				human.transform.localEulerAngles = new Vector3(0, 180, 0);
				
				return;
			}

			if (gameObject.CompareTag("Human") && gameObject.name == go4.name)
			{
				Vector3 position = gameObject.transform.localPosition;
				var parent = gameObject.transform.parent;
				
				DestroyImmediate(gameObject, true);
				
				var human = Instantiate(go4) as GameObject;
				human.name = human.name.Replace("(Clone)", "");
				human.transform.parent = parent;
				human.transform.localPosition = position;
				human.transform.localEulerAngles = new Vector3(0, 180, 0);
				
				return;
			}

			if (gameObject.CompareTag("Human") && gameObject.name == go5.name)
			{
				Vector3 position = gameObject.transform.localPosition;
				var parent = gameObject.transform.parent;
				
				DestroyImmediate(gameObject, true);
				
				var human = Instantiate(go5) as GameObject;
				human.name = human.name.Replace("(Clone)", "");
				human.transform.parent = parent;
				human.transform.localPosition = position;
				human.transform.localEulerAngles = new Vector3(0, 180, 0);
				
				return;
			}
			
			foreach (Transform child in gameObject.transform) 
			{
				Adjustment(child.gameObject);
			}
		}
	}
}
