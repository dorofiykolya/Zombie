using UnityEngine;
using System.Collections;
using Runner;
using UnityEditor;
[CustomEditor(typeof(Runner.ObstacleObject))]
[CanEditMultipleObjects]
public class ObstacleObjectEditor : LocationObjectEditor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}
