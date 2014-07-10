using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Runner;
[CustomEditor(typeof(Runner.Obstacle))]
[CanEditMultipleObjects]
public class ObstacleEditor : LocationObjectEditor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}
