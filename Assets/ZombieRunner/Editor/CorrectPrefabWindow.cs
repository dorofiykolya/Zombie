using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;
public class CorrectPrefabWindow : EditorWindow {

	private int correctedDuplicates;
	private int correctedLocationObjects;

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

		if (GUILayout.Button ("REMOVE BLOOM FROM LOCATION OBJECT")) 
		{
			correctedLocationObjects = 0;
			var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
			foreach(var go in gameObjects)
			{
				FixBloom(go);
			}
		}
		GUILayout.Label ("bloom result: " + correctedLocationObjects);
		if (GUILayout.Button ("REMOVE ANIMATOR FROM GAME OBJECTS")) 
		{
			var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
			foreach(var go in gameObjects)
			{
				FixAnimator(go);
			}
		}
		if (GUILayout.Button ("REMOVE MeshExploder FROM GAME OBJECTS")) 
		{
			var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
			foreach(var go in gameObjects)
			{
				FixExplode(go);
			}
		}
	}

	void FixAnimator(GameObject go)
	{
		var anim = go.GetComponent<Animator> ();

		if(anim != null)
		{
			DestroyImmediate(anim, true);
		}

		foreach (var g in go.GetChildren()) 
		{
			FixAnimator(g);
		}
	}

	void FixExplode(GameObject go)
	{
		var expl = go.GetComponent<MeshExploder> ();
		
		if(expl != null)
		{
			DestroyImmediate(expl, true);
		}
		
		foreach (var g in go.GetChildren()) 
		{
			FixExplode(g);
		}
	}

	void FixBloom(GameObject go)
	{
		var lo = go.GetComponent<Runner.LocationObject> ();
		if (lo != null) {
			var bloom = go.GetComponent<Bloom>();
			if(bloom != null)
			{
				DestroyImmediate(bloom, true);
				correctedLocationObjects++;
			}
			foreach (var g in go.GetChildren()) {
				FixBloom(g);
			}
		}
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
