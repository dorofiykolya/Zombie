using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PlatformObject))]
[CanEditMultipleObjects]
public class LocationObjectEditor : Editor
{
	public LocationManager location;

	public override void OnInspectorGUI()
	{
		location = GameObject.FindObjectOfType<Runner.LocationManager>();
		if (location == null)
		{
			GUI.color = Color.red;
			GUILayout.Label("ERROR, LocationManager not found!!!");
			GUI.color = Color.white;
			return;
		}

		var locationObject = target as LocationObject;
		if (locationObject != null) 
		{

		}
	}

	protected void DrawBoolLabel(string label, bool value)
	{
		GUILayout.BeginHorizontal();
		if (value)
		{
			GUI.color = Color.grey;
			GUILayout.Label(label, GUILayout.Width(150.0f));
			GUI.color = Color.cyan;
			GUILayout.Label("True");
			GUI.color = Color.white;
		}
		else
		{
			GUI.color = Color.grey;
			GUILayout.Label(label, GUILayout.Width(150.0f));
			GUILayout.Label("False");
			GUI.color = Color.white;
		}
		GUILayout.EndHorizontal();
	}
}


