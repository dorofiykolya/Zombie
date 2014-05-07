using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;
public class CorrectPrefabWindow : EditorWindow {

	private int correctedDuplicates;

	void OnGUI()
	{
		if (GUILayout.Button ("REMOVE DUPLICATE LOCATION OBJECT")) 
		{
			correctedDuplicates = 0;
			var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
			foreach(var go in gameObjects)
			{
				FixDuplicateLO(go);
			}
		}
		GUILayout.Label ("duplicate result: " + correctedDuplicates);
		/*var selected = Selection.gameObjects;
		foreach (var s in selected) 
		{
			var result = s.gameObject.GetComponentsInChildren<Component>(true).Where(r => r==null).ToArray();
			if(result.Length > 0)
			{
				EditorGUILayout.LabelField(result.Length.ToString());
			}
		}*/
	}

	void FixDuplicateLO(GameObject go)
	{
		var los = go.GetComponents<Runner.LocationObject> ().ToList();
		while(los.Count > 1) {
			DestroyImmediate(los[los.Count -1], true);
			los.RemoveAt(los.Count -1);
			correctedDuplicates++;
		}
		foreach (var g in go.GetChildren()) {
			FixDuplicateLO(g);
		}
	}

	[MenuItem("Runner/CorrectPrefab")]
	public static void ShowCorrectPrefabWindow()
	{
		EditorWindow.GetWindow<CorrectPrefabWindow>("CorrectPrefab", true).Show();
	}
}
